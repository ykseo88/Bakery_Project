using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StackCarrier : StackContainer
{
    private string INTERRACTIONZONE_TAG = "InteractionZone";

    public bool allowInput = true;
    public bool allowOutPut = true;
    
    private bool isaleadyMoveable = false;

    private bool isContackted = false;
    
    public event Action<EStackableObjects> IsFinishGetEvent;
    public event Action<EStackableObjects> IsFinishGiveEvent;
    
    // Start is called before the first frame update

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(INTERRACTIONZONE_TAG))
        {
            isContackted = true;
            Debug.Log($"{other.gameObject.name}과 상호작용");
            other.transform.TryGetComponent(out StackContainer stackContainer);
            
            //둘 다 빈손인지 확인
            if (stackContainer.currentStackObjectType == EStackableObjects.None
                && currentStackObjectType == EStackableObjects.None)
            {
                //Debug.LogError($"{gameObject.name}양측 빈손임!");
                return;
            }
            
            //운반자가 이 창고에 들고있는 물건을 넣을 수 있는지 확인
            bool isInputAble = mainField.CheackInputAble(this, stackContainer, currentStackObjectType) 
                               && CheckMoveable(stackContainer, true);
            //운반자가 창고에서 물건을 꺼낼 수 있는지 확인
            bool isOutputAble = mainField.CheackOutputAble(this, stackContainer, stackContainer.currentStackObjectType)  
                                && CheckMoveable(stackContainer, false);

            if (isInputAble && allowInput && isaleadyMoveable == false && stackContainer.stackMovealbe)
            {
                
                GiveObject(stackContainer.stackPoint.position, stackContainer, CheckCurrentStackableObjectIsNone());
            }
            else
            {
                Debug.Log($"{gameObject.name}못 넣음!");
            }
            
            if (isOutputAble && allowOutPut && isaleadyMoveable == false && stackContainer.stackMovealbe)
            {
                GetObject(stackPoint.position, stackContainer, stackContainer.CheckCurrentStackableObjectIsNone());
            }
            else
            {
                Debug.Log($"{gameObject.name}못 꺼냄!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(INTERRACTIONZONE_TAG)) isContackted = false;
    }

    private IEnumerator GiveStart(Vector3 end, StackContainer stackContainer, bool isDeActive)
    {
        stackContainer.stackMovealbe = false;
        isaleadyMoveable = true;
        while (currentStack.Count > 0 && stackContainer.GetCurrentStack().Count < stackContainer.maxStackNum)
        {
            StackableObject tempSObj = GiveStackObject();
            if(tempSObj.autoGrid != null) tempSObj.autoGrid.OutElement(tempSObj.transform);
            tempSObj.transform.SetParent(null);
            tempSObj.CheckParentGrid();
            tempSObj.MoveStackableObject(tempSObj.transform.position, end, stackContainer, this, isDeActive);
            yield return new WaitForSeconds(mainField.putTerm);
        }
        isaleadyMoveable = false;
        stackContainer.stackMovealbe = true;
        IsFinishGiveEvent?.Invoke(currentStackObjectType);
    }

    private IEnumerator GetStart(Vector3 end, StackContainer stackContainer, bool isDeActive)
    {
        stackContainer.stackMovealbe = false;
        isaleadyMoveable = true;
        while (stackContainer.GetCurrentStack().Count > 0 && currentStack.Count < maxStackNum)
        {
            StackableObject tempSObj = stackContainer.GiveStackObject();
            if(tempSObj.autoGrid != null) tempSObj.autoGrid.OutElement(tempSObj.transform);
            tempSObj.transform.SetParent(null);
            tempSObj.CheckParentGrid();
            tempSObj.MoveStackableObject(tempSObj.transform.position, end, this, stackContainer, isDeActive);
            yield return new WaitForSeconds(mainField.putTerm);
        }
        isaleadyMoveable = false;
        stackContainer.stackMovealbe = true;
        IsFinishGetEvent?.Invoke(currentStackObjectType);
    }

    public void GetObject(Vector3 end, StackContainer stackContainer, bool isDeActive = false)
    {
        StartCoroutine(GetStart(end, stackContainer, isDeActive));
    }
    
    public void GiveObject(Vector3 end, StackContainer stackContainer, bool isDeActive = false)
    {
        StartCoroutine(GiveStart(end, stackContainer, isDeActive));
    }

    private bool CheckMoveable(StackContainer stackContainer, bool isInput)
    {
        if (isInput)
        {
            if (currentStackObject.isNoneStack == false)
            {
                return CheckNotMissMatch(stackContainer);
            }
        }
        else
        {
            if (stackContainer.currentStackObject.isNoneStack == false)
            {
                return CheckNotMissMatch(stackContainer);
            }
        }

        return true;
    }

    private bool CheckNotMissMatch(StackContainer stackContainer)
    {
        //둘 다 물건을 들고 있는데 양쪽 물건이 다른지 확인
        if ((stackContainer.currentStackObjectType != EStackableObjects.None
             && currentStackObjectType != EStackableObjects.None)
            && currentStackObjectType != stackContainer.currentStackObjectType)
        {
            Debug.LogError($"{gameObject.name}양측 물건이 다름!");
            return false;
        }
            
        //양쪽 다 꽉 찼는지 확인
        if (stackContainer.GetIsFullStack() && GetIsFullStack())
        {
            //Debug.LogError($"{gameObject.name}양측 꽉참!");
            return false;
        }

        return true;
    }
}
