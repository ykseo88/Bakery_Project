using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackContackZone : MonoBehaviour
{
    [SerializeField] private StackContainer stackContainer;
    [SerializeField] private float contackSize;
    

    private bool isContack = false;

    private void Update()
    {
        
    }

    public StackContainer GetStackContainer()
    {
        return stackContainer;
    }

    public void ToggleContack()
    {
        isContack = !isContack;
        OnEffectContack();
    }

    private void OnEffectContack()
    {
        
    }
    
}
