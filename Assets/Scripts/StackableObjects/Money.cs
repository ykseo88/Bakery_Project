using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : StackableObject
{
    // Start is called before the first frame update
    void Start()
    {
        type = EStackableObjects.Money;
        isNoneStack = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
