using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegacyInputManager : MonoBehaviour
{
    [SerializeField] private Image virtualJoyStick;
    [SerializeField] private Image handle;
    [SerializeField] private Image touchPoint;
    public event Action<bool> isTouchedEvent;
    public Vector2 screenTouchPos;
    public Vector3 worldTouchPos;
    
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
                        screenTouchPos = touch.position;
                        worldTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
                        touchPoint.rectTransform.anchoredPosition = screenTouchPos;
                        touchPoint.enabled = true;
                    }
                    break;
                case TouchPhase.Moved:
                    screenTouchPos = touch.position;
                    worldTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
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
