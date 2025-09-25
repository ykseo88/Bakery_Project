using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaitingSlot
{
    public WaitngPoint waitPoint;
    public bool isWait;
    public CustomerController waitCustomerController;
    public StackCarrier customerStackCarrier;

    public WaitingSlot(WaitngPoint waitPoint, bool isWait,CustomerController customerController)
    {
        this.waitPoint = waitPoint;
        this.isWait = isWait;
        this.waitCustomerController = customerController;
    }
}

public class WaitingQueue : MonoBehaviour
{
    public List<WaitingSlot> waitingSlots = new List<WaitingSlot>();
    public Queue<CustomerController> waitQueue = new Queue<CustomerController>();
    public int maxWaitingSlotNum = 3;
    public int currentWaitingNum = 0;
    public bool isWaitable = true;
    
    public CustomerController currentCustomer;

    protected virtual void Start()
    {
        
    }
    
    protected virtual void Update()
    {
        UpdateIsWaitable();
    }

    public void Add(CustomerController customerController)
    {
        waitingSlots[waitQueue.Count].waitCustomerController = customerController;
        waitQueue.Enqueue(customerController);
        //if(waitQueue.Count == 0) currentCustomer = customerController;
        //waitQueue.Enqueue(customerController);
    }

    public void UpdateIsWaitable()
    {
        if(waitQueue.Count >= maxWaitingSlotNum) isWaitable = false;
        else isWaitable = true;
        
        currentWaitingNum = waitQueue.Count;
    }

    public WaitingSlot GetFreeWaitingSlot()
    {
        foreach (WaitingSlot waitingSlot in waitingSlots)
        {
            if(waitingSlot.isWait == false) return waitingSlot;
        }

        return null;
    }

    public bool ContainCustomer(CustomerController customer)
    {
        foreach (CustomerController tempCustomer in waitQueue)
        {
            if (tempCustomer.Equals(customer)) return true;
        }

        return false;
    }

    public WaitingSlot GetWaitingSlotCurrentCustomer()
    {
        foreach (WaitingSlot waitingSlot in waitingSlots)
        {
            if(waitingSlot.waitCustomerController == currentCustomer) return waitingSlot;
        }
        
        return null;
    }
}
