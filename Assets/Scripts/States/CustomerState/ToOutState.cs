using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class ToOutState : ICustomerState
{
    private enum ESequance
    { ToStopOverPoint, ToWaitPoint }
    
    private static readonly int STACK_WALK = Animator.StringToHash("StackWalk");
    private const float rotateDuration = 0.1f;
    private CustomerController customerController;
    
    private Animator animator;
    private StackContainer stacable;
    private NavMeshAgent agent;
    private Vector3 waitPosition;
    private Quaternion waitRotation;
    private Vector3 wayPoint;
    private Vector3 stopOverPoint;
    private ESequance currentSequance;
    
    public ToOutState(CustomerController customerController) => this.customerController =  customerController;
    
    
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.ToOutState);
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        animator.SetTrigger(STACK_WALK);
        stopOverPoint = customerController.CustomerManager.cashDesk.customerOutPoint.position;
        wayPoint = stopOverPoint;
        agent.SetDestination(wayPoint);
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
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            switch (currentSequance)
            {
                case ESequance.ToStopOverPoint:
                    wayPoint = waitPosition;
                    agent.SetDestination(customerController.CustomerManager.transform.position);
                    currentSequance = ESequance.ToWaitPoint;
                    break;
                case ESequance.ToWaitPoint:
                    PoolManager.instance.DeActiveObject(customerController.gameObject);
                    break;
            }
        }
    }
}
