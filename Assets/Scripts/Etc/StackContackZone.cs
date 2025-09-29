using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackContackZone : MonoBehaviour
{
    [SerializeField] private StackContainer stackContainer;
    
    public StackContainer GetStackContainer()
    {
        return stackContainer;
    }
}
    
