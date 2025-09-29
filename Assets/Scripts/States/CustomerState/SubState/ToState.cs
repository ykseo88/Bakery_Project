using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class ToState : IState
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
    
    protected Transform currentWayPoint;
    protected Queue<Transform> wayPoints = new Queue<Transform>(); 
    
    protected float Distance;
    
    public virtual void Enter()
    {
        animator = customerController.Animator;
        agent = customerController.NavMeshAgent;
        carrier = customerController.StackCarrier;
        agent.updateRotation = false;
        agent.enabled = true;
        
        animator.ResetTrigger(STACK_IDLE);
        animator.ResetTrigger(IDLE);
        
        if(carrier.isHasStack)animator.SetTrigger(STACK_WALK);
        else animator.SetTrigger(WALK);
    }

    public virtual void Update()
    {
        
    }

    public virtual void Exit()
    {
        if(carrier.isHasStack)animator.ResetTrigger(STACK_WALK);
        else animator.ResetTrigger(WALK);
        agent.velocity = Vector3.zero;
    }

    protected void CheckArrivePoint(IState nextState = null)
    {
        customerController.transform.DOLookAt(customerController.transform.position + agent.velocity.normalized, rotateDuration, AxisConstraint.Y);
        if (currentWayPoint == null)
        {
            if (wayPoints.Count == 0) return;
            
            currentWayPoint = wayPoints.Dequeue();
            agent.SetDestination(currentWayPoint.position);
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWayPoint = null;
            if (wayPoints.Count == 0)
            {
                customerController.ChangeState(nextState);
            }
        }
    }

    public void EnqueueWayPoints(Transform point)
    {
        wayPoints.Enqueue(point);
    }
}
