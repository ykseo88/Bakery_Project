using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class WaitBreadState : IUnitState
{
    private const string POS = "Pos";
    
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int STACK_IDLE = Animator.StringToHash("StackIdle");
    private const string BREAD = "Bread";
    private const float rotateDuration = 0.5f;
    
    private CustomerController customerController;
    public WaitBreadState(CustomerController customerController) => this.customerController = customerController;
    

    private Animator animator;
    private AnimatorStateInfo stateInfo;
    private StackContainer stacable;
    private NavMeshAgent agent;
    private EatTable eatTable;
    

    private bool isGoTable = false;
    
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.WaitBreadState);
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        animator.SetTrigger(IDLE);
        customerController.stateBubble.SetActive(true);
        customerController.markWithNumber.enabled = true;
        customerController.numberText.enabled = true;
        customerController.markWithNumber.sprite = customerController.MainField.GetSpriteByName(BREAD);
        customerController.SetIsArrivedQueuePoint(true);
        eatTable = customerController.CustomerManager.eatTable;
        float randFloat = Random.Range(0, 1f);
        isGoTable = randFloat < customerController.MainField.goTableProbability;
    }

    public void Update()
    {
        customerController.numberText.SetText((customerController.wantBreadNum - customerController.StackCarrier.currentStackNum).ToString());
        if(stacable.isHasStack) animator.SetTrigger(STACK_IDLE);
        if (customerController.CheckFullGetBread())
        {
            if (isGoTable)
            {
                if (eatTable.IsOpen)
                {
                    customerController.ChangeState((new GetTableState(customerController, true)));
                }
                else
                {
                    customerController.ChangeState(new ToTableState(customerController));
                }
            }
            else customerController.ChangeState(new ToCashDeskState(customerController));
            
        }
        customerController.transform.DOLookAt(customerController.CustomerManager.showBasket.transform.position, rotateDuration, AxisConstraint.Y);
    }

    public void Exit()
    {
        customerController.markWithNumber.enabled = false;
        customerController.numberText.enabled = false;
        customerController.currentCustomerWantMark.enabled = true;
        customerController.currentCustomerWantMark.sprite = customerController.MainField.GetSpriteByName(POS);
        
        animator.ResetTrigger(IDLE);
        animator.ResetTrigger(STACK_IDLE);
    }
}
