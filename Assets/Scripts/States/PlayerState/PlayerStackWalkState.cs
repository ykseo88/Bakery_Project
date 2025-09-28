using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStackWalkState : IUnitState
{
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int STACK_WALK = Animator.StringToHash("StackWalk");
    private PlayerController playerController;
    public PlayerStackWalkState(PlayerController playerController) => this.playerController = playerController;
    private Animator animator;
    private StackContainer stacable;
    
    public void Enter()
    {
        animator = playerController.animator;
        playerController.transform.TryGetComponent(out stacable);
        animator.SetTrigger(STACK_WALK);
    }

    public void Update()
    {
        if (stacable.isHasStack)
        {
            if (playerController.GetMoveValue() == Vector2.zero)
            {
                playerController.ChangeState(new PlayerStackIdleState(playerController));
            }
        }
        else
        {
            if(playerController.GetMoveValue() != Vector2.zero)
                playerController.ChangeState(new PlayerWalkState(playerController));
            else
                playerController.ChangeState(new PlayerIdleState(playerController));
        }
        
        
    }

    public void Exit()
    {
        animator.ResetTrigger(STACK_WALK);
    }
}
