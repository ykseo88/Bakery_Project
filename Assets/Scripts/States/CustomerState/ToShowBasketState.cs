using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class ToShowBasketState : ICustomerState
{
    private enum Sequance
    { toStopOverPoint, toWaitPoint }
    
    private static readonly int WALK = Animator.StringToHash("Walk");
    private const float rotateDuration = 0.1f;

    
    private CustomerController customerController;
    public ToShowBasketState(CustomerController customerController) => this.customerController = customerController;

    private Animator animator;
    private StackContainer stacable;
    private NavMeshAgent agent;
    private Vector3 waitPosition;
    private Quaternion waitRotation;
    private Vector3 wayPoint;
    private Vector3 stopOverPoint;
    private float Distance;
    private ShowBasket showBasket;
    
    private Sequance currentSeauance = Sequance.toStopOverPoint;
    
    
    
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.ToShowBasketState);
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        animator.SetTrigger(WALK);
        showBasket = customerController.CustomerManager.showBasket;
        showBasket.Enqueue(customerController);
        WaitingSlot waitingSlot = showBasket.GetWaitingSlotOrNullByCustomer(customerController);
        waitPosition = waitingSlot.waitPoint.transform.position;
        waitRotation = waitingSlot.waitPoint.transform.rotation;
        stopOverPoint = customerController.CustomerManager.centerPoint.position;
        wayPoint = stopOverPoint;
        agent.SetDestination(wayPoint);
        customerController.OnImpactEvent += ResetDestination;
    }

    public void Update()
    {
        customerController.transform.DOLookAt(customerController.transform.position + agent.velocity.normalized, rotateDuration, AxisConstraint.Y);
        CheckArrivePoint();
    }

    public void Exit()
    {
        animator.ResetTrigger(WALK);
        
    }

    private void ResetDestination()
    {
        agent.SetDestination(wayPoint);
    }

    private void CheckArrivePoint()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
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