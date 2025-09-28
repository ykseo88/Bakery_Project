using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class WaitTableStete : WaitState
{
    private const string TABLE = "Table";
    public WaitTableStete(CustomerController customerController) => this.customerController = customerController;
    private EatTable eatTable;
    
    public override void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.WaitTableStete);
        
        base.Enter();
        
        eatTable = customerController.CustomerManager.eatTable;
        
        customerController.currentCustomerWantMark.sprite = customerController.MainField.GetSpriteByName(TABLE);
        
        eatTable.UsingUpdateEvent += UpdateWaitPoint;
    }

    public override void Update()
    {

        switch (eatTable.IsOpen && !eatTable.IsDirty)
        {
            case true:
                if (eatTable.UsingCustomer == null) return;
                CheckWait(new GetTableState(customerController), eatTable.FoodPoint.position,
                    eatTable.UsingCustomer.Equals(customerController));
                break;
            case false:
                customerController.transform.DOLookAt(customerController.transform.position + Vector3.back, rotateDuration, AxisConstraint.Y);
                
                break;
        }
    }

    public override void Exit()
    {
        base.Exit();
        eatTable.UsingUpdateEvent -= UpdateWaitPoint;
    }

    private void UpdateWaitPoint()
    {
        currentWaitingSlot = eatTable.GetWaitingSlotOrNullByCustomer(customerController);
        agent.SetDestination(currentWaitingSlot.waitPoint.transform.position);
        animator.ResetTrigger(STACK_IDLE);
        animator.SetTrigger(STACK_WALK);
    }
}