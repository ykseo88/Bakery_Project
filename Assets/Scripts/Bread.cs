using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : StackableObject
{
    public BreadMaker breadMaker;
    private bool isfirst = true;
    private Collider col;

    private void Start()
    {
        transform.TryGetComponent(out col);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isfirst)
        {
            isfirst = false;
            breadMaker.ActiveBakeable();
            breadMaker.transform.TryGetComponent(out StackInven stackInven);
            Debug.Log("푸쉬?" + stackInven);
            stackInven.GetStackObject(this);
        }
    }
}
