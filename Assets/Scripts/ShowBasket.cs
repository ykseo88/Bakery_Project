using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBasket : WaitingQueue
{
    private StackContainer showBasketContainer;

    protected override void Start()
    {
        base.Start();
        transform.TryGetComponent(out showBasketContainer);
    }

    protected override void Update()
    {
        base.Update();
        SetNewCurrentCustomer();
        AllowGetBread();
    }

    private void SetNewCurrentCustomer()
    {
        if (currentCustomer != null)
        {
            if (currentCustomer.CheckFullGetBread())
            {
                currentCustomer.customerManager.allowSpawnNum++;
                GetWaitingSlotCurrentCustomer().isWait = false;
                GetWaitingSlotCurrentCustomer().waitCustomerController = null;
                var element = waitQueue.Dequeue();
                if(waitQueue.Count > 0) currentCustomer = waitQueue.Peek();
                Debug.Log("1번 교체");
            }
        }
    }

    private void AllowGetBread()
    {
        if (currentCustomer !=  null)
        {
            currentCustomer.transform.TryGetComponent(out StackCarrier customerStack);
            if(customerStack.allowOutPut == false) customerStack.allowOutPut = true;
        }
    }
}
