using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class ToCashDeskState : IUnitState
{
    private enum Sequance
    { toStopOverPoint, toWaitPoint }
    
    private const float arriveDistance = 1.5f;
    private const float rotateDuration = 0.1f;
    
    private static readonly int WALK = Animator.StringToHash("Walk");
    private static readonly int STACK_WALK = Animator.StringToHash("StackWalk");
    private CustomerController customerController;
    private Transform waitTransform;
    public ToCashDeskState(CustomerController customerController) => this.customerController = customerController;

    private Animator animator;
    private StackCarrier stacable;
    private NavMeshAgent agent;
    private Vector3 waitPosition;
    private Quaternion waitRotation;
    private Vector3 wayPoint;
    private Vector3 stopOverPoint;
    private float Distance;
    private CashDesk cashDesk;
    
    private Sequance currentSeauance = Sequance.toStopOverPoint;

    private float Ditance;
    
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.ToCashDeskState);
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        cashDesk = customerController.CustomerManager.cashDesk;
        if(cashDesk.Count == 0) cashDesk.SetWaitingState();
        cashDesk.Enqueue(customerController);
        WaitingSlot waitingSlot = cashDesk.GetWaitingSlotOrNullByCustomer(customerController);
        waitPosition = waitingSlot.waitPoint.transform.position;
        waitRotation = waitingSlot.waitPoint.transform.rotation;
        stopOverPoint = customerController.CustomerManager.centerPoint.position;
        wayPoint = stopOverPoint;
        agent.SetDestination(wayPoint);
        stacable.allowOutPut = false;
        stacable.allowInput = false;
        animator.SetTrigger(STACK_WALK);
        customerController.SetIsArrivedQueuePoint(true);
        agent.updateRotation = false;
    }

    public void Update()
    {
        customerController.transform.DOLookAt(customerController.transform.position + agent.velocity.normalized, rotateDuration, AxisConstraint.Y);
        CheckArrivePoint();
    }

    public void Exit()
    {
        
    }
    
    private void CheckArrivePoint()
    {
        switch (currentSeauance)
        {
            case Sequance.toStopOverPoint:
                if (agent.remainingDistance <= arriveDistance)
                {
                    wayPoint = waitPosition;
                    agent.SetDestination(wayPoint);
                    currentSeauance = Sequance.toWaitPoint;
                }
                break;
            case Sequance.toWaitPoint:
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    customerController.ChangeState(new WaitPayState(customerController));
                }
                break;
        }
        
        
    }
}
