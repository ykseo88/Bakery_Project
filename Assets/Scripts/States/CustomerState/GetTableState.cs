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
        base.Enter();
        customerController.SetDebugCurrentState(ECustomerStates.GetTableState);
        
        eatTable = customerController.CustomerManager.eatTable;

        foreach (Transform stopOverPoint in eatTable.StopOverPointArray)
        {
            wayPoints.Enqueue(stopOverPoint);
        }
    }

    public override void Update()
    {
        CheckArrivePoint(new EatingState(customerController));
    }

    public override void Exit()
    {
        
    }
}
