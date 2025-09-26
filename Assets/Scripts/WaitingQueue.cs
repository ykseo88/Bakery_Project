using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class WaitingSlot
{
    public WaitngPoint waitPoint;
    public CustomerController Customer { get; set; }
    
    public bool IsEmpty => Customer == null;
}

public class WaitingQueue : MonoBehaviour
{
    public List<WaitingSlot> waitingSlots = new List<WaitingSlot>();
    public int maxWaitingSlotNum = 3;
    private Queue<WaitingSlot> waitQueue = new Queue<WaitingSlot>();
    
    public int  Count => waitQueue.Count;
    public bool IsFull => GetEmptyWaitingSlot() == null;
    
    private WaitingSlot GetEmptyWaitingSlot()
    {
        return waitingSlots.FirstOrDefault(t => t.IsEmpty);
    }
    
    public WaitingSlot Enqueue(CustomerController customerController)
    {
        var waitingSlot = GetEmptyWaitingSlot();
        if (waitingSlot == null)
        {
            //동적 추가를 하든 뭘 하든 처리
            return null;
        }
        else
        {
            waitingSlot.Customer = customerController;
            waitQueue.Enqueue(waitingSlot);
            return waitingSlot;
        }
    }

    public WaitingSlot Dequeue()
    {
        return waitQueue.Dequeue();
    }

    public bool ContainCustomer(CustomerController customer)
    {
        return waitingSlots.Find(x=>x.Customer == customer) != null;
    }
    
    
}
