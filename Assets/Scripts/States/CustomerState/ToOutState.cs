using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class ToOutState : ToState
{
    public ToOutState(CustomerController customerController) => this.customerController =  customerController;
    
    private CashDesk cashDesk;
    
    public override void Enter()
    {
        base.Enter();
        cashDesk = customerController.CustomerManager.cashDesk;
        wayPoints.Enqueue(cashDesk.customerOutPoint);
        wayPoints.Enqueue(customerController.CustomerManager.transform);
    }

    public override void Update()
    {
        CheckArrivePoint();
    }

    public override void Exit()
    {
        base.Exit();
        PoolManager.instance.DeActiveObject(customerController.gameObject);
    }
}
