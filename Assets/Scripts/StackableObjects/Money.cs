using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : StackableObject
{
    
    
    void Start()
    {
        type = EStackableObjects.Money;
        isNoneStack = true;
    }


    void Update()
    {
        
    }
}
