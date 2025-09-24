using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackCarrier : StackContainer
{
    private string INTERRACTIONZONE_TAG = "InteractionZone";
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(INTERRACTIONZONE_TAG))
        { 
            Debug.Log($"{other.gameObject.name}과 상호작용");
            other.transform.TryGetComponent(out StackContainer stackContainer);
            
            //둘 다 물건을 들고 있는데 양쪽 물건이 다른지 확인
            if ((stackContainer.currentStackObject != EStackableObjects.None
                 && currentStackObject != EStackableObjects.None)
                && currentStackObject != stackContainer.currentStackObject)
            {
                Debug.LogError($"양측 물건이 다름!");
                return;
            }
            
            //둘 다 빈손인지 확인
            if (stackContainer.currentStackObject == EStackableObjects.None
                && currentStackObject == EStackableObjects.None)
            {
                Debug.LogError($"양측 빈손임!");
                return;
            }
            
            //양쪽 다 꽉 찼는지 확인
            if (stackContainer.GetIsFullStack() && GetIsFullStack())
            {
                Debug.LogError($"양측 꽉참!");
                return;
            }
            
            //운반자가 이 창고에 들고있는 물건을 넣을 수 있는지 확인
            bool isInputAble = mainField.CheackInputAble(this, stackContainer, currentStackObject);
            //운반자가 창고에서 물건을 꺼낼 수 있는지 확인
            bool isOutputAble = mainField.CheackOutputAble(this, stackContainer, stackContainer.currentStackObject);

            if (isInputAble)
            {
                StartCoroutine(GiveStart(stackContainer.stackPoint.position, stackContainer));
            }
            else
            {
                Debug.Log($"못 넣음!");
            }
            
            if (isOutputAble)
            {
                StartCoroutine(GetStart(stackPoint.position, stackContainer));
            }
            else
            {
                Debug.Log($"못 꺼냄!");
            }
        }
    }

    private IEnumerator GiveStart(Vector3 end, StackContainer stackContainer)
    {
        while (currentStack.Count > 0 && stackContainer.GetCurrentStack().Count < stackContainer.maxStackNum)
        {
            StackableObject tempSObj = GiveStackObject();
            tempSObj.MoveStackableObject(tempSObj.transform.position, end, stackContainer, this);
            yield return new WaitForSeconds(mainField.putTerm);
        }
    }

    private IEnumerator GetStart(Vector3 end, StackContainer stackContainer)
    {
        while (stackContainer.GetCurrentStack().Count > 0 && currentStack.Count < maxStackNum)
        {
            StackableObject tempSObj = stackContainer.GiveStackObject();
            tempSObj.MoveStackableObject(tempSObj.transform.position, end, this, stackContainer);
            yield return new WaitForSeconds(mainField.putTerm);
        }
    }
}
