using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EatingState : IState
{
    private static readonly int Sit = Animator.StringToHash("Sitting_Talking");
    private static readonly int WALK = Animator.StringToHash("Defalut_Walk");
    private static float offsetY = 0.5f;
    
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
        
        animator.Play(Sit);
    }

    public void Update()
    {
        customerController.transform.DOLookAt(eatTable.FoodPoint.position + Vector3.down * offsetY, 0.1f);
        if(eatTable.UsingCustomer == null) customerController.ChangeState(new ToOutState(customerController, eatTable.OutPoint));
    }

    public void Exit()
    {
        animator.Play(WALK);
    }
}
