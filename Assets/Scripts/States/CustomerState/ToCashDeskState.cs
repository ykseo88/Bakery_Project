using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToCashDeskState : ICustomerState
{
    private static readonly int WALK = Animator.StringToHash("Walk");
    private CustomerController customerController;
    private Transform waitTransform;
    public ToCashDeskState(CustomerController customerController) => this.customerController = customerController;

    private Animator animator;
    private StackContainer stacable;
    private NavMeshAgent agent;
    private Vector3 wayPoint;

    private bool toShowBasket = false;
    
    public void Enter()
    {
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        animator.SetTrigger(WALK);
        wayPoint = customerController.CustomerManager.centerPoint.position;
        agent.SetDestination(wayPoint);
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        
    }
}
