using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class ToShowBasketState : ToState
{
    public ToShowBasketState(CustomerController customerController) => this.customerController = customerController;

    private ShowBasket showBasket;
    
    public override void Enter()
    {
        base.Enter();
        
        showBasket = customerController.CustomerManager.showBasket;
        showBasket.Enqueue(customerController);
        WaitingSlot waitingSlot = showBasket.GetWaitingSlotOrNullByCustomer(customerController);
        wayPoints.Enqueue(customerController.CustomerManager.centerPoint);
        wayPoints.Enqueue(waitingSlot.waitPoint.transform);
    }

    public override void Update()
    {
        CheckArrivePoint(new WaitBreadState(customerController));
    }

    public override void Exit()
    {
        base.Exit();
    }
}