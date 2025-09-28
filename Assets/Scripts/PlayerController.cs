using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitController
{
    [SerializeField] private SAOMainField mainField;
    [SerializeField] private InputManager inputManager;
    public Animator animator;
    [SerializeField] private JoystickController joystickController;
    [SerializeField] private GameObject playerModel;
    private Vector2 moveValue;
    private Vector2 rotateValue;

    protected override void Start()
    {
        base.Start();
        ChangeState(new PlayerIdleState(this));
        transform.TryGetComponent(out stackCarrier);
    }
    
    protected override void Update()
    {
        base.Update();
        
        if (joystickController.isOnJoystick)
        {
            moveValue = joystickController.GetInputVector();
            rotateValue = joystickController.GetInputVector();
        }
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
    
    public void ChangeState(IUnitState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public Vector2 GetMoveValue()
    {
        return moveValue;
    }

    public StackCarrier GetStackContainer()
    {
        return stackCarrier;
    }
}
