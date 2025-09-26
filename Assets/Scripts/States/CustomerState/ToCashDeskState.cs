using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToCashDeskState : ICustomerState
{
    private const float arriveDistance = 1.5f;
    private static readonly int WALK = Animator.StringToHash("Walk");
    private static readonly int STACK_WALK = Animator.StringToHash("StackWalk");
    private CustomerController customerController;
    private Transform waitTransform;
    public ToCashDeskState(CustomerController customerController) => this.customerController = customerController;

    private Animator animator;
    private StackCarrier stacable;
    private NavMeshAgent agent;
    private Vector3 wayPoint;

    private float Ditance;
    
    public void Enter()
    {
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        stacable.allowOutPut = false;
        stacable.allowInput = false;
        animator.SetTrigger(STACK_WALK);
        wayPoint = customerController.CustomerManager.tempPoint.position;
        agent.SetDestination(wayPoint);
    }

    public void Update()
    {
    }

    public void Exit()
    {
        
    }
}
