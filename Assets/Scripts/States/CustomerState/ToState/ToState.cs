using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class ToState : IUnitState
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
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void Exit()
    {
        if(carrier.isHasStack)animator.ResetTrigger(STACK_WALK);
        else animator.ResetTrigger(WALK);
    }

    protected void CheckArrivePoint(IUnitState nextState = null)
    {
        customerController.transform.DOLookAt(customerController.transform.position + agent.velocity.normalized, rotateDuration, AxisConstraint.Y);
        if (currentWayPoint == null)
        {
            if (wayPoints.Count == 0) return;
            
            currentWayPoint = wayPoints.Dequeue();
            agent.SetDestination(currentWayPoint.position);

            if(carrier.isHasStack)animator.SetTrigger(STACK_WALK);
            else animator.SetTrigger(WALK);

        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log($"현재 향하는 곳: 위치 {agent.destination}, 커렌웨이포인트{currentWayPoint}");
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
