using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class ToCashDeskState : ToState
{
    public ToCashDeskState(CustomerController customerController) => this.customerController = customerController;
    
    private StackCarrier stacable;
    private CashDesk cashDesk;
    
    
    public override void Enter()
    {
        base.Enter();
        
        stacable = customerController.StackCarrier;
        cashDesk = customerController.CustomerManager.cashDesk;
        
        if(cashDesk.Count == 0) cashDesk.SetWaitingState();
        cashDesk.Enqueue(customerController);
        
        WaitingSlot waitingSlot = cashDesk.GetWaitingSlotOrNullByCustomer(customerController);
        wayPoints.Enqueue(customerController.CustomerManager.centerPoint);
        wayPoints.Enqueue(waitingSlot.waitPoint.transform);
        
        stacable.allowOutPut = false;
        stacable.allowInput = false;

        customerController.SetIsArrivedQueuePoint(true);
        
    }

    public override void Update()
    {
        CheckArrivePoint(new WaitPayState(customerController));
    }

    public override void Exit()
    {
        
    }
}
