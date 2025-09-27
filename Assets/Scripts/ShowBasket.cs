using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBasket : WaitingQueue
{
    private StackContainer showBasketContainer;
    private CustomerController usingCustomer;
    
    protected void Awake()
    {
        transform.TryGetComponent(out showBasketContainer);
    }

    protected void Update()
    {
        if (usingCustomer != null)
        {
            if (usingCustomer.CheckFullGetBread())
            {
                usingCustomer.CustomerManager.allowSpawnNum++;
                GetWaitingSlotOrNullByCustomer(usingCustomer).Customer = null;
                usingCustomer = null;
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
            
            usingCustomer = Dequeue();
            usingCustomer.transform.TryGetComponent(out StackCarrier customerStack);
            
            if (customerStack.allowOutPut == false)
            {
                customerStack.allowOutPut = true;
            }
        }
    }
}
