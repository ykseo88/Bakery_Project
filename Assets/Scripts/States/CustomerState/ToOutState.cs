using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class ToOutState : ToState
{
    public ToOutState(CustomerController customerController) => this.customerController = customerController;
    
    private CashDesk cashDesk;
    
    public override void Enter()
    {
        base.Enter();
        customerController.SetDebugCurrentState(ECustomerStates.ToOutState);
        customerController.stateBubble.SetActive(false);
        cashDesk = customerController.CustomerManager.cashDesk;
        wayPoints.Enqueue(cashDesk.customerOutPoint);
        wayPoints.Enqueue(customerController.CustomerManager.transform);
    }

    public override void Update()
    {
        CheckArrivePoint(new NoneState(customerController));
    }

    public override void Exit()
    {
        base.Exit();
        PoolManager.instance.DeActiveObject(customerController.gameObject);
    }
}
