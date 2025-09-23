using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : IPlayerState
{
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int WALK = Animator.StringToHash("Walk");
    private PlayerController playerController;
    public PlayerWalkState(PlayerController playerController) => this.playerController = playerController;
    private Animator animator;
    private StackContainer stacable;
    
    public void Enter()
    {
        animator = playerController.animator;
        playerController.transform.TryGetComponent(out stacable);
        animator.SetTrigger(WALK);
    }

    public void Update()
    {
        if (stacable.isHasStack)
        {
            if (playerController.GetMoveValue() == Vector2.zero)
            {
                playerController.ChangeState(new PlayerStackIdleState(playerController));
            }
            else
            {
                playerController.ChangeState(new PlayerStackWalkState(playerController));
            }
        }
        else if(playerController.GetMoveValue() == Vector2.zero)
            playerController.ChangeState(new PlayerIdleState(playerController));
        
        
    }

    public void Exit()
    {
        animator.ResetTrigger(WALK);
    }
}
