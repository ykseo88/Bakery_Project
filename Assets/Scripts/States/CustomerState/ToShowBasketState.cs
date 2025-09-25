using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToShowBasketState : ICustomerState
{
    private static readonly int WALK = Animator.StringToHash("Walk");
    private CustomerController customerController;
    private Transform waitTransform;
    public ToShowBasketState(CustomerController customerController) => this.customerController = customerController;

    private Animator animator;
    private StackContainer stacable;
    private NavMeshAgent agent;
    private Vector3 waitPosition;
    private Vector3 wayPoint;

    private bool toShowBasket = false;
    
    public void Enter()
    {
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        animator.SetTrigger(WALK);
        WaitingSlot waitingSlot = customerController.CustomerManager.showBasket.GetEmptyWaitingSlot();
        waitPosition = waitingSlot.waitPoint.transform.position;
        wayPoint = customerController.CustomerManager.centerPoint.position;
        agent.SetDestination(wayPoint);
        customerController.OnImpactEvent += ResetDestination();
    }

    public void Update()
    {
        if (agent.remainingDistance <= 1f && !toShowBasket)
        {
            wayPoint = waitPosition;
            agent.SetDestination(wayPoint);
            toShowBasket = true;
        }
        
        if (agent.remainingDistance <= 1f && toShowBasket)
        {
            customerController.ChangeState(new WaitBreadState(customerController));
        }
    }

    public void Exit()
    {
        animator.ResetTrigger(WALK);
    }

    private Action ResetDestination()
    {
        agent.SetDestination(wayPoint);
        return null;
    }
}