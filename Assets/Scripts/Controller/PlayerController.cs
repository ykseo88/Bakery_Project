using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : UnitController
{
    [SerializeField] private InputManager inputManager;
    public InputManager InputManager => inputManager;
    public Animator animator;
    [SerializeField] private JoystickController joystickController;
    [SerializeField] private GameObject playerModel;
    private Vector2 moveValue;
    private Vector2 rotateValue;
    [SerializeField]private CameraStay stay;
    public CameraStay Stay => stay;

    public bool isHandle = true;
    
    [SerializeField] TMP_Text maxText;

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
        UpdateMaxStackNotice();
    }

    public void UpdateMoveValue(Vector2 inputVector)
    {
        moveValue = inputVector;
    }

    private void UpdateMove()
    {
        if (!isHandle) return;
        transform.position += new Vector3(moveValue.x, 0, moveValue.y) * mainField.playerSpeed;
    }

    private void UpdateRotation()
    {
        if (!isHandle) return;
        playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(rotateValue.x, 0, rotateValue.y).normalized, Vector3.up);
    }
    
    public void ChangeState(IState newState)
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
    private void UpdateMaxStackNotice()
    {
        if (stackCarrier.currentStackNum >= stackCarrier.maxStackNum && maxText.enabled == false)
        {
            maxText.enabled = true;
        }
        else if (stackCarrier.currentStackNum < stackCarrier.maxStackNum && maxText.enabled == true)
        {
            maxText.enabled = false;
        }
    }

    public void SetJoystick(bool isOn)
    {
        inputManager.enabled = isOn;
    }
    
}
