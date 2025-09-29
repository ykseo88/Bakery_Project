using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Compass : MonoBehaviour
{
    private const float Minimum = 0.01f;
    
    [SerializeField] private GameObject compassArrow;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private float compassArrowDistance;

    private void Start()
    {
        SetUpdateCompassPos(compassArrowDistance);
        tutorialManager.compass = this;
    }

    void Update()
    {
        UpdateCurrentArrowDirection();
    }

    private void UpdateCurrentArrowDirection()
    {
        Vector3 direction = tutorialManager.CurrentArrowTransform.position - transform.position;
        direction.y = 0;
            
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        
        transform.rotation = targetRotation;

    }

    private void SetUpdateCompassPos(float distance)
    {
        compassArrow.transform.localPosition = new Vector3(compassArrow.transform.localPosition.x, compassArrow.transform.localPosition.y, distance);
    }

    public void OffArrow()
    {
        compassArrow.SetActive(false);
    }
    
    
}
