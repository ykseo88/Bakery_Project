using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SAOMainField mainField;
    [SerializeField] private InputManager inputManager;
    public Animator animator;
    [SerializeField] private JoystickController joystickController;
    [SerializeField] private GameObject playerModel;
    private Vector2 moveValue;
    private Vector2 rotateValue;
    private IPlayerState currentState;

    private void Start()
    {
        ChangeState(new PlayerIdleState(this));
    }
    
    

    private void Update()
    {
        if (joystickController.isOnJoystick)
        {
            moveValue = joystickController.GetInputVector();
            rotateValue = joystickController.GetInputVector();
        }
        currentState.Update();
        UpdateMove();
        UpdateRotation();
    }

    public void UpdateMoveValue(Vector2 inputVector)
    {
        moveValue = inputVector;
    }

    private void UpdateMove()
    {
        transform.position += new Vector3(moveValue.x, 0, moveValue.y) * mainField.playerSpeed;
    }

    private void UpdateRotation()
    {
        playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(rotateValue.x, 0, rotateValue.y).normalized, Vector3.up);
    }
    
    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
        Debug.Log($"현재 상태: {currentState}");
    }

    public Vector2 GetMoveValue()
    {
        return moveValue;
    }
    
    
}
