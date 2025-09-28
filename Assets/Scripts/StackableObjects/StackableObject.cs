using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStackableObjects
{
    None,
    Bread,
    Money,
    PaperBag,
}

public class StackableObject : MonoBehaviour
{
    public EStackableObjects type;
    [SerializeField] private SAOMainField mainField;
    public AutoGrid autoGrid;
    [SerializeField] private float putTimeRate = 1f;
    [SerializeField] private float putTermRate = 1f;
    
    public float PutTimeRate => putTimeRate;
    public float PutTermRate => putTermRate;

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
        float time = mainField.putTime * putTimeRate;
        
        Vector3 destination = toStackContainer.transform.position;
        Quaternion destRotation = toStackContainer.transform.rotation; // Default rotation

        if (toStackContainer.autoGrid != null)
        {
            destination = toStackContainer.autoGrid.nextEmptyWorldPosition;
            destRotation = Quaternion.Euler(toStackContainer.autoGrid.objRotation);
        }
        
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            transform.position = toStackContainer.GetBezierPoint(start, toStackContainer.autoGrid.nextEmptyWorldPosition, currentTime / mainField.putTime);
            transform.rotation = Quaternion.Lerp(originRot, Quaternion.Euler(toStackContainer.autoGrid.objRotation), currentTime / mainField.putTime);
            yield return null;
        }

        
        IsFinishMoveEvent?.Invoke(this);
        
        //Debug.Log($"받는 곳: {toStackContainer.gameObject.name}, 물건 종류: {type}, 현재 받는 곳 타입: {toStackContainer.currentStackObject}");
        if (isDeActive)
        {
            PoolManager.instance.DeActiveObject(this.gameObject);
        }
        else
        {
            toStackContainer.currentStackObjectType = type;
            toStackContainer.GetStackObject(this, false);
        }

        if (fromStackContainer != null && fromStackContainer.autoGrid != null)
        {
            fromStackContainer.autoGrid.UpdateElements();
        }
        
        //Debug.Log($"{this}가 {fromStackContainer}에서 {toStackContainer}로 이동됨. 현재 {toStackContainer}의 스택 수는 {toStackContainer.GetCurrentStack().Count}개!");
    }

    public virtual void MoveStackableObject(Vector3 start, StackContainer toStackContainer, StackContainer fromStackContainer, bool isDeActive)
    {
        StartCoroutine(MoveCoroutine(start, toStackContainer, fromStackContainer, isDeActive));
    }

    private void OnDestroy()
    {
        Debug.LogError($"{gameObject.name} 부서짐!");
    }
}
