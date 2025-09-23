using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackCarrier : StackContainer
{
    private string INTERRACTIONZONE_TAG = "InteractionZone";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(INTERRACTIONZONE_TAG))
        { 
            Debug.Log($"{other.gameObject.name}과 상호작용");
            other.transform.root.TryGetComponent(out StackContainer stackContainer);
            
            //둘 다 물건을 들고 있는데 양쪽 물건이 다른지 확인
            if((stackContainer.currentStackObject != EStackableObjects.None
               && currentStackObject != EStackableObjects.None)
               && currentStackObject != stackContainer.currentStackObject) return;
            
            //둘 다 빈손인지 확인
            if (stackContainer.currentStackObject == EStackableObjects.None
                && currentStackObject == EStackableObjects.None) return;
            
            //양쪽 다 꽉 찼는지 확인
            if (stackContainer.GetIsFullStack() && GetIsFullStack()) return;
            
            //운반자가 이 창고에 들고있는 물건을 넣을 수 있는지 확인
            bool isInputAble = mainField.CheackInputAble(this, stackContainer, currentStackObject);
            //운반자가 창고에서 물건을 꺼낼 수 있는지 확인
            bool isOutputAble = mainField.CheackOutputAble(this, stackContainer, stackContainer.currentStackObject);
            
            if(isInputAble)
                while (currentStack.Count <= 0)
                {
                    StackableObject stackableObject = GiveStackObject();
                    stackContainer.GetStart(stackableObject, stackableObject.transform.position, stackPoint.position,
                        stackContainer);
                }
            if(isOutputAble)
                while (currentStack.Count <= 0)
                {
                    StackableObject stackableObject = stackContainer.GiveStackObject();
                    GetStart(stackableObject, stackableObject.transform.position, stackPoint.position,
                        this);
                }
        }
    }
}
