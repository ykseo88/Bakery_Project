using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Image virtualJoyStick;
    [SerializeField] private Image handle;
    [SerializeField] private Image touchPoint;
    public event Action<bool> isTouchedEvent;
    public Vector2 touchPos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateJoyStickPos();
    }

    private void UpdateJoyStickPos()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (i == 0)
                    {
                        isTouchedEvent?.Invoke(true);
                        touchPos = touch.position;
                        touchPoint.rectTransform.anchoredPosition = touchPos;
                        touchPoint.enabled = true;
                    }
                    break;
                case TouchPhase.Moved:
                    touchPos = touch.position;
                    touchPoint.rectTransform.anchoredPosition = touch.position;
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    if (i == Input.touchCount - 1)
                    {
                        isTouchedEvent?.Invoke(false);
                        touchPoint.enabled = false;
                    }
                    break;
            }
        }
    }
    
}
