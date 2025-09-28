using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EatingState : IUnitState
{
    private static readonly int Sit = Animator.StringToHash("Sit");
    
    private CustomerController customerController;
    
    public EatingState(CustomerController customerController) => this.customerController = customerController;
    
    private Animator animator;
    private NavMeshAgent agent;
    private StackCarrier carrier;
    
    private EatTable eatTable;
    
    
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.EatingState);
        customerController.transform.TryGetComponent(out animator);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out carrier);
        
        eatTable = customerController.CustomerManager.eatTable;
        
        agent.enabled = false;
        
        customerController.transform.position = eatTable.SitPoint.position;
        customerController.transform.rotation = eatTable.SitPoint.localRotation;
        
        animator.SetTrigger(Sit);
    }

    public void Update()
    {
        customerController.transform.position = eatTable.SitPoint.position;
        customerController.transform.rotation = eatTable.SitPoint.localRotation;
    }

    public void Exit()
    {
        
    }
}
