using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class WaitPayState : ICustomerState
{
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int STACK_IDLE = Animator.StringToHash("StackIdle");
    private const float rotateDuration = 0.5f;
    
    private CustomerController customerController;
    public WaitPayState(CustomerController customerController) => this.customerController = customerController;
    

    private Animator animator;
    private AnimatorStateInfo stateInfo;
    private StackContainer stacable;
    private NavMeshAgent agent;
    private WaitingSlot currentWaitingSlot;
    private CashDesk cashDesk;
    private bool isGoTable;
    
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.WaitPayState);
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        animator.SetTrigger(STACK_IDLE);
        cashDesk = customerController.CustomerManager.cashDesk;
        cashDesk.PaymentCompletedEvent += UpdateWaitPoint;
        isGoTable = Random.Range(0f, 1f) <= customerController.CustomerManager.GetGoTableProbability();
        currentWaitingSlot = cashDesk.GetWaitingSlotOrNullByCustomer(customerController);
        customerController.transform.DOLookAt(customerController.transform.position + Vector3.back, rotateDuration, AxisConstraint.Y);
        UpdateWaitPoint();
    }

    public void Update()
    {
        if (customerController.CheckGetPaperBag())
        {
            customerController.ChangeState(new ToOutState(customerController));
        }
    }

    public void Exit()
    {
        cashDesk.PaymentCompletedEvent -= UpdateWaitPoint;
    }

    private void UpdateWaitPoint()
    {
        currentWaitingSlot = cashDesk.GetWaitingSlotOrNullByCustomer(customerController);
        agent.SetDestination(currentWaitingSlot.waitPoint.transform.position);
    }
}
