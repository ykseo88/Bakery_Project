using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitPayState : ICustomerState
{
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int STACK_IDLE = Animator.StringToHash("StackIdle");
    private CustomerController customerController;
    public WaitPayState(CustomerController customerController) => this.customerController = customerController;
    

    private Animator animator;
    private AnimatorStateInfo stateInfo;
    private StackContainer stacable;
    private NavMeshAgent agent;
    private bool isGoTable;
    
    public void Enter()
    {
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        animator.SetTrigger(STACK_IDLE);
        agent.updateRotation = false;
        isGoTable = Random.Range(0f, 1f) <= customerController.CustomerManager.GetGoTableProbability();
    }

    public void Update()
    {
        if (customerController.CheckGetPaperBag())
        {
            if (isGoTable)
            {
                customerController.ChangeState(new ToTableState(customerController));
            }
            else
            {
                customerController.ChangeState(new ToOutState(customerController));
            }
        }
    }

    public void Exit()
    {
        
    }
}
