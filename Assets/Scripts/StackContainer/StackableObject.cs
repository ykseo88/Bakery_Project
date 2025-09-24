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

    protected virtual void Start()
    {
        CheckParentGrid();
    }

    public void CheckParentGrid()
    {
        if(transform.parent != null) transform.parent.TryGetComponent(out autoGrid);
    }
    
    protected IEnumerator MoveCoroutine(Vector3 start, Vector3 end, StackContainer toStackContainer, StackContainer fromStackContainer)
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

        toStackContainer.GetStackObject(this);
        //Debug.Log($"{this}가 {fromStackContainer}에서 {toStackContainer}로 이동됨. 현재 {toStackContainer}의 스택 수는 {toStackContainer.GetCurrentStack().Count}개!");
    }

    public virtual void MoveStackableObject(Vector3 start, Vector3 end, StackContainer toStackContainer, StackContainer fromStackContainer)
    {
        StartCoroutine(MoveCoroutine(start, end, toStackContainer, fromStackContainer));
    } 
}
