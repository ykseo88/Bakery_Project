using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class StackCarrier : StackContainer
{
    private string INTERRACTIONZONE_TAG = "InteractionZone";

    public bool allowInput = true;
    public bool allowOutPut = true;
    
    private bool isAleadyMoveable = false;
    public bool IsAleadyMoveable => isAleadyMoveable;

    private bool isContacted = true;
    public bool IsContacted => isContacted;
    
    public event Action<EStackableObjects> IsFinishGetEvent;
    public event Action<EStackableObjects> IsFinishGiveEvent;
    
    public event Action<StackableObject> IsStartOneMoveEvent;
    public event Action IsDoneMoveEvent;
    
    // Start is called before the first frame update

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(INTERRACTIONZONE_TAG))
        {
            isContacted = true;
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

            if (isInputAble && allowInput && IsAleadyMoveable == false && stackContainer.stackMovealbe)
            {
                
                GiveObject(stackContainer, CheckCurrentStackableObjectIsNone());
            }
            else
            {
                //Debug.Log($"{gameObject.name}못 넣음!");
            }
            
            if (isOutputAble && allowOutPut && IsAleadyMoveable == false && stackContainer.stackMovealbe)
            {
                GetObject(stackContainer, stackContainer.CheckCurrentStackableObjectIsNone());
            }
            else
            {
                //Debug.Log($"{gameObject.name}못 꺼냄!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(INTERRACTIONZONE_TAG)) isContacted = false;
    }

    private IEnumerator GiveStart(StackContainer stackContainer, bool isDeActive, GameObject prefab)
    {
        stackContainer.stackMovealbe = false;
        isAleadyMoveable = true;
        while (currentStack.Count > 0 && stackContainer.GetCurrentStack().Count < stackContainer.maxStackNum && isContacted)
        {
            StackableObject tempSObj = GiveStackObject();
            IsStartOneMoveEvent?.Invoke(tempSObj);
            if(tempSObj.autoGrid != null) tempSObj.autoGrid.OutElement(tempSObj.transform);
            tempSObj.transform.SetParent(null);
            tempSObj.CheckParentGrid();
            tempSObj.MoveStackableObject(tempSObj.transform.position, stackContainer, this, isDeActive);
            yield return new WaitForSeconds(mainField.putTerm);
        }
        isAleadyMoveable = false;
        stackContainer.stackMovealbe = true;
        IsFinishGiveEvent?.Invoke(currentStackObjectType);
    }

    private IEnumerator GetStart(StackContainer stackContainer, bool isDeActive, GameObject prefab)
    {
        stackContainer.stackMovealbe = false;
        isAleadyMoveable = true;
        while (stackContainer.GetCurrentStack().Count > 0 && currentStack.Count < maxStackNum && isContacted)
        {
            StackableObject tempSObj = stackContainer.GiveStackObject();
            IsStartOneMoveEvent?.Invoke(tempSObj);
            if(tempSObj.autoGrid != null) tempSObj.autoGrid.OutElement(tempSObj.transform);
            tempSObj.transform.SetParent(null);
            tempSObj.CheckParentGrid();
            tempSObj.MoveStackableObject(tempSObj.transform.position, this, stackContainer, isDeActive);
            yield return new WaitForSeconds(mainField.putTerm);
        }
        isAleadyMoveable = false;
        stackContainer.stackMovealbe = true;
        IsFinishGetEvent?.Invoke(currentStackObjectType);
    }

    public void GetObject(StackContainer stackContainer, bool isDeActive = false, GameObject prefab = null)
    {
        StartCoroutine(GetStart(stackContainer, isDeActive, prefab));
    }
    
    public void GiveObject(StackContainer stackContainer, bool isDeActive = false,  GameObject prefab = null)
    {
        StartCoroutine(GiveStart(stackContainer, isDeActive, prefab));
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

    public void SetIsContact(bool isContact)
    {
        isContacted = isContact;
    }
}
