using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class WaitPayState : WaitState
{ 
    public WaitPayState(CustomerController customerController) => this.customerController = customerController;
    
    private CashDesk cashDesk;
    
    public override void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.WaitPayState);
        
        base.Enter();
        
        cashDesk = customerController.CustomerManager.cashDesk;
        cashDesk.UsingUpdateEvent += UpdateWaitPoint;
        
        currentWaitingSlot = cashDesk.GetWaitingSlotOrNullByCustomer(customerController);
        
        UpdateWaitPoint();
    }

    public override void Update()
    {
        CheckWait(new ToOutState(customerController, cashDesk.customerOutPoint), customerController.transform.position + Vector3.back, customerController.CheckGetPaperBag());

    }

    public override void Exit()
    {
        base.Exit();
        cashDesk.UsingUpdateEvent -= UpdateWaitPoint;
    }

    private void UpdateWaitPoint()
    {
        currentWaitingSlot = cashDesk.GetWaitingSlotOrNullByCustomer(customerController);
        agent.SetDestination(currentWaitingSlot.waitPoint.transform.position);
        animator.ResetTrigger(STACK_IDLE);
        animator.SetTrigger(STACK_WALK);
    }
}
