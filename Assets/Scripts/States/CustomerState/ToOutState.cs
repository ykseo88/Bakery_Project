using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class ToOutState : ToState
{
    private Transform _outPoint;

    private const string WALK_ANI = "Default_Walk";
    private const string STACK_WALK_ANI = "Stack_Walk";

    public ToOutState(CustomerController customerController, Transform outPoint)
    {
        this.customerController = customerController;
        _outPoint = outPoint;
    } 
    
    private CashDesk cashDesk;
    
    public override void Enter()
    {
        base.Enter();
        agent.enabled = true;
        customerController.SetDebugCurrentState(ECustomerStates.ToOutState);
        customerController.stateBubble.SetActive(false);
        cashDesk = customerController.CustomerManager.cashDesk;
        wayPoints.Enqueue(_outPoint);
        wayPoints.Enqueue(customerController.CustomerManager.transform);
        if(customerController.StackCarrier.isHasStack)animator.Play(STACK_WALK_ANI);
        else animator.Play(WALK_ANI);
        
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
