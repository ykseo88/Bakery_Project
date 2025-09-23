using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SAOMainField mainField;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Animator animator;
    [SerializeField] private JoystickController joystickController;
    [SerializeField] private GameObject playerModel;
    private Vector2 moveValue;

    private void Start()
    {

    }

    private void Update()
    {
        if (joystickController.isOnJoystick)
        {
            moveValue = joystickController.GetInputVector();
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
        playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(moveValue.x, 0, moveValue.y).normalized, Vector3.up);
    }

    private void UpdateAnimation()
    {
        
    }
    
    
}
