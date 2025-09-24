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
    
    protected IEnumerator MoveCoroutine(Vector3 start, Vector3 end, StackContainer toStackContainer, StackContainer fromStackContainer)
    {
        float currentTime = 0f;
        
        while (currentTime < mainField.putTime)
        {
            currentTime += Time.deltaTime;
            transform.position = toStackContainer.GetBezierPoint(start, toStackContainer.stackPoint.transform.position, currentTime / mainField.putTime);
            yield return null;
        }

        toStackContainer.GetStackObject(this);
        //Debug.Log($"{this}가 {fromStackContainer}에서 {toStackContainer}로 이동됨. 현재 {toStackContainer}의 스택 수는 {toStackContainer.GetCurrentStack().Count}개!");
    }

    public virtual void MoveStackableObject(Vector3 start, Vector3 end, StackContainer toStackContainer, StackContainer fromStackContainer)
    {
        StartCoroutine(MoveCoroutine(start, end, toStackContainer, fromStackContainer));
    } 
}
