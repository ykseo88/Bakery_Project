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

    protected virtual void Start()
    {
        CheckParentGrid();
    }

    public void CheckParentGrid()
    {
        if(transform.parent != null) transform.parent.TryGetComponent(out autoGrid);
    }
    
    protected IEnumerator MoveCoroutine(Vector3 start, Vector3 end, StackContainer toStackContainer, StackContainer fromStackContainer, bool isDeActive)
    {
        float currentTime = 0f;
        Quaternion originRot =  transform.rotation;
        
        while (currentTime < mainField.putTime)
        {
            currentTime += Time.deltaTime;
            transform.position = toStackContainer.GetBezierPoint(start, toStackContainer.autoGrid.nextEmptyWorldPosition, currentTime / mainField.putTime);
            transform.rotation = Quaternion.Lerp(originRot, Quaternion.Euler(toStackContainer.autoGrid.objRotation), currentTime / mainField.putTime);
            yield return null;
        }
        
        
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

    public virtual void MoveStackableObject(Vector3 start, Vector3 end, StackContainer toStackContainer, StackContainer fromStackContainer, bool isDeActive)
    {
        StartCoroutine(MoveCoroutine(start, end, toStackContainer, fromStackContainer, isDeActive));
    }
}
