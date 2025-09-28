using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class WaitTableStete : IUnitState
{
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int STACK_IDLE = Animator.StringToHash("StackIdle");
    private static readonly int STACK_WALK = Animator.StringToHash("StackWalk");
    private const float rotateDuration = 0.5f;
    
    private CustomerController customerController;
    public WaitTableStete(CustomerController customerController) => this.customerController = customerController;
    

    private Animator animator;
    private AnimatorStateInfo stateInfo;
    private StackContainer stacable;
    private NavMeshAgent agent;
    private WaitingSlot currentWaitingSlot;
    private EatTable eatTable;
    private bool isUsable = false;
    
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.WaitTableStete);
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        animator.SetTrigger(STACK_IDLE);
        eatTable = customerController.CustomerManager.eatTable;
        eatTable.PaymentCompletedEvent += UpdateWaitPoint;
        currentWaitingSlot = eatTable.GetWaitingSlotOrNullByCustomer(customerController);
        customerController.transform.DOLookAt(customerController.transform.position + Vector3.back, rotateDuration, AxisConstraint.Y);
        UpdateWaitPoint();
        if(eatTable.IsOpen) isUsable = true;
    }

    public void Update()
    {

        switch (isUsable)
        {
            case true:
                if (eatTable.UsingCustomer.Equals(customerController))
                {
                    customerController.ChangeState(new GetTableState(customerController));
                }
                break;
            case false:
                // 테이블 안 열렸을 때 처리
                break;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.ResetTrigger(STACK_WALK);
            animator.SetTrigger(STACK_IDLE);
        }
    }

    public void Exit()
    {
        eatTable.PaymentCompletedEvent -= UpdateWaitPoint;
    }

    private void UpdateWaitPoint()
    {
        currentWaitingSlot = eatTable.GetWaitingSlotOrNullByCustomer(customerController);
        agent.SetDestination(currentWaitingSlot.waitPoint.transform.position);
        animator.ResetTrigger(STACK_IDLE);
        animator.SetTrigger(STACK_WALK);
    }
}
