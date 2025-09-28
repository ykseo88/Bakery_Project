using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class WaitState : IUnitState
{
    protected const float rotateDuration = 0.1f;
    protected static readonly int IDLE = Animator.StringToHash("Idle");
    protected static readonly int WALK = Animator.StringToHash("Walk");
    protected static readonly int STACK_WALK = Animator.StringToHash("StackWalk");
    protected static readonly int STACK_IDLE = Animator.StringToHash("StackIdle");
    
    protected CustomerController customerController;
    
    protected Animator animator;
    protected NavMeshAgent agent;
    protected StackCarrier carrier;
    
    protected WaitingSlot currentWaitingSlot;
    
    public virtual void Enter()
    {
        animator = customerController.Animator;
        agent = customerController.NavMeshAgent;
        carrier = customerController.StackCarrier;
        agent.updateRotation = false;
        agent.enabled = true;
        
        if(carrier.isHasStack)animator.SetTrigger(STACK_IDLE);
        else animator.SetTrigger(IDLE);
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void Exit()
    {
        if(carrier.isHasStack)animator.ResetTrigger(STACK_IDLE);
        else animator.ResetTrigger(IDLE);
    }

    protected void CheckWait(IUnitState nextState, Vector3 seeAngle, bool isMyTurn = false)
    {
        customerController.transform.DOLookAt(seeAngle, rotateDuration, AxisConstraint.Y);
        
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if(carrier.isHasStack)animator.ResetTrigger(STACK_WALK);
            else animator.ResetTrigger(WALK);
            
            if(carrier.isHasStack)animator.SetTrigger(STACK_IDLE);
            else animator.SetTrigger(IDLE);
        }
        
        if (isMyTurn)
        {
            customerController.ChangeState(nextState);
        }
    }
}
