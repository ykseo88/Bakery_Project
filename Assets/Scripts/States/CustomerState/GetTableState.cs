using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class GetTableState : ToState
{
    private bool isRight;
    public GetTableState(CustomerController customerController, bool isRight = false)
    {
        this.customerController = customerController;
        this.isRight = isRight;
    }
    
    private EatTable eatTable;
    
    public override void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.GetTableState);
        
        customerController.stateBubble.SetActive(false);
        
        eatTable = customerController.CustomerManager.eatTable;
        
        if(eatTable.Count == 0) eatTable.SetWaitingState();

        if (isRight)
        {
            eatTable.Enqueue(customerController);
            wayPoints.Enqueue(eatTable.SitPoint);
        }
        else
        {
            foreach (Transform stopOverPoint in eatTable.StopOverPointArray)
            {
                wayPoints.Enqueue(stopOverPoint);
            }
        }

        
        
        base.Enter();
        customerController.SetDebugCurrentState(ECustomerStates.GetTableState);
        
        
    }

    public override void Update()
    {
        CheckArrivePoint(new EatingState(customerController));
    }

    public override void Exit()
    {
        
    }
}
