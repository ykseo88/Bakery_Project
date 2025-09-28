using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStackableObjects
{
    None,
    Bread,
    Money,
    PaperBag
}

public class StackableObject : MonoBehaviour
{
    public EStackableObjects type;
    [SerializeField] private SAOMainField mainField;
    public AutoGrid autoGrid;

    public bool isNoneStack = false;
    
    public event Action<StackableObject> IsFinishMoveEvent;

    protected virtual void Start()
    {
        CheckParentGrid();
    }

    public void CheckParentGrid()
    {
        if(transform.parent != null) transform.parent.TryGetComponent(out autoGrid);
    }
    
    protected IEnumerator MoveCoroutine(Vector3 start, StackContainer toStackContainer, StackContainer fromStackContainer, bool isDeActive)
    {
        Debug.Log($"MoveCoroutine started for {name}");
        float currentTime = 0f;
        Quaternion originRot =  transform.rotation;
        
        Vector3 destination = toStackContainer.transform.position;
        Quaternion destRotation = toStackContainer.transform.rotation; // Default rotation

        if (toStackContainer.autoGrid != null)
        {
            destination = toStackContainer.autoGrid.nextEmptyWorldPosition;
            destRotation = Quaternion.Euler(toStackContainer.autoGrid.objRotation);
        }

        Debug.Log($"MoveCoroutine loop starting for {name}. putTime: {mainField.putTime}");
        while (currentTime < mainField.putTime)
        {
            currentTime += Time.deltaTime;
            transform.position = toStackContainer.GetBezierPoint(start, destination, currentTime / mainField.putTime);
            transform.rotation = Quaternion.Lerp(originRot, destRotation, currentTime / mainField.putTime);
            yield return null;
        }
        Debug.Log($"MoveCoroutine loop finished for {name}");
        
        IsFinishMoveEvent?.Invoke(this);
        Debug.Log($"IsFinishMoveEvent invoked for {name}");
        
        //Debug.Log($"받는 곳: {toStackContainer.gameObject.name}, 물건 종류: {type}, 현재 받는 곳 타입: {toStackContainer.currentStackObject}");
        if (isDeActive)
        {
            PoolManager.instance.DeActiveObject(this.gameObject);
        }
        else
        {
            toStackContainer.currentStackObjectType = type;
            toStackContainer.GetStackObject(this);
        }
        
        //Debug.Log($"{this}가 {fromStackContainer}에서 {toStackContainer}로 이동됨. 현재 {toStackContainer}의 스택 수는 {toStackContainer.GetCurrentStack().Count}개!");
    }

    public virtual void MoveStackableObject(Vector3 start, StackContainer toStackContainer, StackContainer fromStackContainer, bool isDeActive)
    {
        StartCoroutine(MoveCoroutine(start, toStackContainer, fromStackContainer, isDeActive));
    }
}
