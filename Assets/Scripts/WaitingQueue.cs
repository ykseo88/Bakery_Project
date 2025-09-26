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

    public WaitingSlot(WaitngPoint waitPoint)
    {
        this.waitPoint = waitPoint;
    }
}

public class WaitingQueue : MonoBehaviour
{
    public List<WaitingSlot> waitingSlots = new List<WaitingSlot>();
    public int maxWaitingSlotNum = 3;
    protected Queue<WaitingSlot> waitQueue = new Queue<WaitingSlot>();
    [SerializeField] protected GameObject waitPointPrefab;
    [SerializeField] protected PoolManager poolManager;
    
    public int  Count => waitQueue.Count;
    public bool IsFull => GetEmptyWaitingSlot() == null;

    protected virtual void Start()
    {
        poolManager.SetPoolQueue(waitPointPrefab);
    }

    protected WaitingSlot GetEmptyWaitingSlot()
    {
        for (int i = 0; i < waitingSlots.Count; i++)
        {
            if(waitingSlots[i].IsEmpty) return waitingSlots[i];
        }
        return null;
    }
    
    public virtual WaitingSlot Enqueue(CustomerController customerController)
    {
        var waitingSlot = GetEmptyWaitingSlot();
        if (waitingSlot == null && waitingSlots.Count < maxWaitingSlotNum)
        {
            poolManager.ActiveObject(waitPointPrefab).transform.TryGetComponent(out WaitngPoint waitPoint);
            waitPoint.transform.SetParent(transform);
            WaitingSlot tempWaitingSlot = new WaitingSlot(waitPoint);
            waitingSlots.Add(tempWaitingSlot);
            waitQueue.Enqueue(tempWaitingSlot);
            return tempWaitingSlot;
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

    protected bool GetIsEmptyWaitingSlot()
    {
        foreach (WaitingSlot slot in waitingSlots)
        {
            if (slot.IsEmpty) return true;
        }

        return false;
    }
    
    
}
