using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToCashDeskState : ICustomerState
{
    private enum Sequance
    { toStopOverPoint, toWaitPoint }
    
    private const float arriveDistance = 1.5f;
    private const float arriveLineDistance = 0.1f;
    private const float rotateDuration = 0.5f;
    
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
    
    private Sequance currentSeauance = Sequance.toStopOverPoint;

    private float Ditance;
    
    public void Enter()
    {
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        WaitingSlot waitingSlot = customerController.CustomerManager.cashDesk.Enqueue(customerController);
        waitPosition = waitingSlot.waitPoint.transform.position;
        waitRotation = waitingSlot.waitPoint.transform.rotation;
        stopOverPoint = customerController.CustomerManager.centerPoint.position;
        wayPoint = stopOverPoint;
        agent.SetDestination(wayPoint);
        stacable.allowOutPut = false;
        stacable.allowInput = false;
        animator.SetTrigger(STACK_WALK);
        agent.updateRotation = true;
    }

    public void Update()
    {
        Distance = Vector3.Distance(customerController.transform.position, wayPoint);
        CheckArrivePoint(Distance);
    }

    public void Exit()
    {
        
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
                    customerController.ChangeState(new WaitPayState(customerController));
                    break;
            }
        }
    }
}
