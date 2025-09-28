using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class GetTableState : ToState
{
    public GetTableState(CustomerController customerController) => this.customerController = customerController;
    
    private EatTable eatTable;
    
    public override void Enter()
    {
        eatTable = customerController.CustomerManager.eatTable;

        foreach (Transform stopOverPoint in eatTable.StopOverPointArray)
        {
            wayPoints.Enqueue(stopOverPoint);
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
