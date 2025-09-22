using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour
{
    private InputManager inputManager;
    private Image joystick;
    private Image handle;
    private float distance;
    private float touchDistance;
    [SerializeField] private float maxDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        inputManager.isTouchedEvent += UpdateJoystick;
        transform.TryGetComponent(out joystick);
        handle = transform.GetChild(0).GetComponent<Image>();
        joystick.enabled = false;
        handle.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDistance();
        UpdateHandle();
    }

    private void UpdateJoystick(bool isTouched)
    {
        Debug.Log("터치"+isTouched);
        if(isTouched) joystick.rectTransform.anchoredPosition = inputManager.touchPos;
        joystick.enabled = isTouched;
        handle.enabled = isTouched;
    }

    private void UpdateHandle()
    {
        if (handle.enabled)
        {
            Vector2 toTouchPointVector = inputManager.touchPos - joystick.rectTransform.anchoredPosition;
            if(touchDistance < maxDistance) handle.transform.position = inputManager.touchPos;
            else handle.rectTransform.anchoredPosition = toTouchPointVector.normalized * maxDistance;;
        } 
            
    }

    private void UpdateDistance()
    {
        distance = transform.localPosition.magnitude;
        touchDistance = Vector2.Distance(joystick.rectTransform.anchoredPosition, inputManager.touchPos);
    }
}
