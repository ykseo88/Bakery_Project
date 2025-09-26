using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class WaitBreadState : ICustomerState
{
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int STACK_IDLE = Animator.StringToHash("StackIdle");
    private const string BREAD = "Bread";
    private CustomerController customerController;
    public WaitBreadState(CustomerController customerController) => this.customerController = customerController;
    

    private Animator animator;
    private AnimatorStateInfo stateInfo;
    private StackContainer stacable;
    private NavMeshAgent agent;
    
    public void Enter()
    {
        customerController.transform.TryGetComponent(out stacable);
        customerController.transform.TryGetComponent(out agent);
        customerController.transform.TryGetComponent(out animator);
        animator.SetTrigger(IDLE);
        customerController.stateBubble.SetActive(true);
        customerController.markWithNumber.enabled = true;
        customerController.numberText.enabled = true;
        customerController.markWithNumber.sprite = customerController.mainField.GetSpriteByName(BREAD);
    }

    public void Update()
    {
        customerController.numberText.SetText((customerController.wantBreadNum - customerController.stackContainer.currentStackNum).ToString());
        if(stacable.isHasStack) animator.SetTrigger(STACK_IDLE);
        if(customerController.CheckFullGetBread()) customerController.ChangeState(new ToCashDeskState(customerController));
    }

    public void Exit()
    {
        animator.ResetTrigger(IDLE);
        animator.ResetTrigger(STACK_IDLE);
    }
}
