using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBasket : WaitingQueue
{
    private StackContainer showBasketContainer;
    private WaitingSlot usingSlot;
    
    protected void Awake()
    {
        transform.TryGetComponent(out showBasketContainer);
    }

    protected void Update()
    {
        if (usingSlot != null)
        {
            if (usingSlot.Customer.CheckFullGetBread())
            {
                usingSlot.Customer.CustomerManager.allowSpawnNum++;
                usingSlot.Customer = null;
                usingSlot = null;
            }
            else
            {
                // 다 받을떄까지 대기
                return;
            }
        }
        else //슬롯이 Null이면 슬롯을 뽑음
        {
            if (Count == 0) return;
            
            usingSlot = Dequeue();
            usingSlot.Customer.transform.TryGetComponent(out StackCarrier customerStack);
            
            if (customerStack.allowOutPut == false)
            {
                customerStack.allowOutPut = true;
            }
        }
    }
}
