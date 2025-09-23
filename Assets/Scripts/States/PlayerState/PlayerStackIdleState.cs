using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStackIdleState : IPlayerState
{
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int STACK_IDLE = Animator.StringToHash("StackIdle");
    private PlayerController playerController;
    public PlayerStackIdleState(PlayerController playerController) => this.playerController = playerController;
    private Animator animator;
    private Stacable stacable;
    
    public void Enter()
    {
        animator = playerController.animator;
        playerController.transform.TryGetComponent(out stacable);
        animator.SetTrigger(STACK_IDLE);
    }

    public void Update()
    {
        if (stacable.isHasStack)
        {
            if (playerController.GetMoveValue() != Vector2.zero)
            {
                playerController.ChangeState(new PlayerStackWalkState(playerController));
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
        animator.ResetTrigger(STACK_IDLE);
    }
}
