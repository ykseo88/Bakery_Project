using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class ToTableState : ToState
{
    public ToTableState(CustomerController customerController) => this.customerController = customerController;
    
    private EatTable eatTable;
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.ToTableState);
        
        eatTable = customerController.CustomerManager.eatTable;
        
        if(eatTable.Count == 0) eatTable.SetWaitingState();
        eatTable.Enqueue(customerController);
        
        wayPoints.Enqueue(customerController.CustomerManager.centerPoint);
        
        WaitingSlot waitingSlot = eatTable.GetWaitingSlotOrNullByCustomer(customerController);
        
        wayPoints.Enqueue(waitingSlot.waitPoint.transform);
        
        carrier.allowOutPut = false;
        carrier.allowInput = false;
        
        customerController.SetIsArrivedQueuePoint(true);
    }

    public override void Update()
    {
        CheckArrivePoint(new WaitTableStete(customerController));
    }

    public void Exit()
    {
        base.Exit();
    }
}
