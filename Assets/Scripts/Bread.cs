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

    public override void MoveStackableObject(Vector3 start, Vector3 end, StackContainer toStackContainer, StackContainer fromStackContainer, Action<StackableObject> processAfterArrive = null)
    {
        col.enabled = false;
        rb.isKinematic = true;
        base.MoveStackableObject(start, end, toStackContainer, fromStackContainer);
    }

    private void OnDisable()
    {
        isfirst = true;
        col.enabled = true;
        rb.isKinematic = false;
    }
}
