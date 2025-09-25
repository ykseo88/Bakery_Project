using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitngPoint : MonoBehaviour
{
    private WaitingQueue CurrentWaitingQueue;

    private void Start()
    {
        CurrentWaitingQueue = transform.root.GetComponentInChildren<WaitingQueue>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.TryGetComponent(out CustomerController customer) && 
            !CurrentWaitingQueue.ContainCustomer(customer) && 
            customer.CurrentState.GetType() == typeof(WaitBreadState))
        {
            CurrentWaitingQueue.Enqueue(customer);
        }
    }
}
