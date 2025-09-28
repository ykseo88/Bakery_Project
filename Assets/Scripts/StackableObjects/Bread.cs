using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : StackableObject
{
    public BreadMaker breadMaker;
    private bool isfirst = true;

    private Collider col;
    private Rigidbody rb;
    
    [SerializeField] private int onePerPrice;
    public int OnePerPrice => onePerPrice;
    
    private void OnEnable()
    {
        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = true;
    }

    protected override void Start()
    {
        base.Start();
        transform.TryGetComponent(out col);
        transform.TryGetComponent(out rb);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isfirst)
        {
            isfirst = false;
            breadMaker.ActiveBakeable();
            breadMaker.transform.root.TryGetComponent(out StackInven stackInven);
            stackInven.GetStackObject(this);
        }
    }

    public override void MoveStackableObject(Vector3 start, StackContainer toStackContainer, StackContainer fromStackContainer, bool isDeActive)
    {
        col.enabled = false;
        rb.isKinematic = true;
        base.MoveStackableObject(start, toStackContainer, fromStackContainer, isDeActive);
    }

    private void OnDisable()
    {
        isfirst = true;
        col.enabled = true;
        rb.isKinematic = false;
    }
}
