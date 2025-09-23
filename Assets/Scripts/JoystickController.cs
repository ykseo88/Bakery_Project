using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerController playerController;
    private Image joystick;
    [SerializeField] private Image handle;
    [SerializeField] private Image touchPoint;
    public float distance;
    public bool isOnJoystick = false;
    private float touchDistance;
    private float handleScalemultiplier;
    public float maxVectorMultiplier;

    public Vector2 currnetVector = Vector2.zero;
    
    [SerializeField] private float maxDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.TryGetComponent(out joystick);
        joystick.enabled = false;
        handle.enabled = false;
        inputManager.isTouchedEvent += OnJoystick;
        
        joystick.enabled = false;
        handle.enabled = false;
        touchPoint.enabled = false;
        
        handleScalemultiplier = joystick.transform.localScale.x;
        maxVectorMultiplier = maxDistance / joystick.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDistance();
        UpdateHandle();
        UpdateTouchPoint();
    }

    private void OnJoystick(bool isTouched, Vector2 position)
    {
        if (isTouched)
        {
            if (position.y < Vector2.zero.y)
            {
                joystick.rectTransform.anchoredPosition = position;
                SetActiveJoystick(true);
            }
        }
        else
        {
            playerController.UpdateMoveValue(Vector2.zero);
            SetActiveJoystick(false);
        }
        
        
    }

    private void SetActiveJoystick(bool isOn)
    {
        isOnJoystick = isOn;
        joystick.enabled = isOn;
        handle.enabled = isOn;
        touchPoint.enabled = isOn;
    }

    private void UpdateHandle()
    {
        if (isOnJoystick)
        {
            currnetVector = Vector2.ClampMagnitude(inputManager._touchScreenCenterPosition - joystick.rectTransform.anchoredPosition, maxDistance) / handleScalemultiplier;
            handle.rectTransform.anchoredPosition = currnetVector;
        } 
            
    }

    private void UpdateTouchPoint()
    {
        touchPoint.rectTransform.anchoredPosition = inputManager._touchScreenCenterPosition;
    }

    private void UpdateDistance()
    {
        distance = Vector2.Distance(handle.rectTransform.anchoredPosition, inputManager._touchWorldPosition);
        touchDistance = Vector2.Distance(joystick.rectTransform.anchoredPosition, inputManager._touchScreenCenterPosition);
    }

    public Vector2 GetInputVector()
    {
        return currnetVector/maxVectorMultiplier;
    }
}
