using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class ToTableState : ToState
{
    
    
    public ToTableState(CustomerController customerController) => this.customerController = customerController;
    
    private EatTable eatTable;
    public override void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.ToTableState);
        
        base.Enter();
        
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
        if(eatTable.UsingCustomer.Equals(customerController) && eatTable.IsOpen) CheckArrivePoint(new EatingState(customerController));
        else CheckArrivePoint(new WaitTableStete(customerController));
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
