using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToShowBasketState : ICustomerState
{
    private enum Sequance
    { toStopOverPoint, toWaitPoint }
    
    private static readonly int WALK = Animator.StringToHash("Walk");
    private const float arriveDistance = 0.5f;
    
    private CustomerController customerController;
    public ToShowBasketState(CustomerController customerController) => this.customerController = customerController;

    private Animator animator;
    private StackContainer stacable;
    private NavMeshAgent agent;
    private Vector3 waitPosition;
    private Vector3 wayPoint;
    private Vector3 stopOverPoint;
    private float Distance;
    
    private Sequance currentSeauance = Sequance.toStopOverPoint;
    
    
    
    public void Enter()
    {
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        animator.SetTrigger(WALK);
        WaitingSlot waitingSlot = customerController.CustomerManager.showBasket.Enqueue(customerController);
        waitPosition = waitingSlot.waitPoint.transform.position;
        stopOverPoint = customerController.CustomerManager.centerPoint.position;
        wayPoint = stopOverPoint;
        agent.SetDestination(wayPoint);
        customerController.OnImpactEvent += ResetDestination;
    }

    public void Update()
    {
        Distance = Vector3.Distance(customerController.transform.position, wayPoint);
        CheckArrivePoint(Distance);
    }

    public void Exit()
    {
        animator.ResetTrigger(WALK);
    }

    private void ResetDestination()
    {
        agent.SetDestination(wayPoint);
    }

    private void CheckArrivePoint(float distance)
    {
        if (distance <= arriveDistance)
        {
            switch (currentSeauance)
            {
                case Sequance.toStopOverPoint:
                    wayPoint = waitPosition;
                    agent.SetDestination(wayPoint);
                    currentSeauance = Sequance.toWaitPoint;
                    break;
                case Sequance.toWaitPoint:
                    customerController.ChangeState(new WaitBreadState(customerController));
                    break;
            }
        }
    }
}