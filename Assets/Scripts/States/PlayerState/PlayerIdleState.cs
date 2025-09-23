using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private PlayerController playerController;
    public PlayerIdleState(PlayerController playerController) => this.playerController = playerController;
    private Animator animator;
    private Stacable stacable;
    
    public void Enter()
    {
        animator = playerController.animator;
        playerController.transform.TryGetComponent(out stacable);
        animator.SetTrigger(IDLE);
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
        else if(playerController.GetMoveValue() != Vector2.zero)
            playerController.ChangeState(new PlayerWalkState(playerController));
        
    }

    public void Exit()
    {
        animator.ResetTrigger(IDLE);
    }
}
