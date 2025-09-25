using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitBreadState : ICustomerState
{
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private const string BREAD = "Bread";
    private CustomerController customerController;
    public WaitBreadState(CustomerController customerController) => this.customerController = customerController;
    

    private Animator animator;
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
        customerController.NumberText.enabled = true;
    }

    public void Update()
    {
        customerController.markWithNumber.sprite = customerController.mainField.GetSpriteByName(BREAD);
        customerController.NumberText.SetText((customerController.wantBreadNum - customerController.stackContainer.currentStackNum).ToString());
        if(customerController.CheckFullGetBread()) customerController.ChangeState(new ToCashDeskState(customerController));
    }

    public void Exit()
    {
        animator.ResetTrigger(IDLE);
    }
}
