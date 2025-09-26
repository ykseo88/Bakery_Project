using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashDesk : WaitingQueue
{
    private StackContainer showBasketContainer;
    private WaitingSlot usingSlot;
    
    [SerializeField] private GameObject paperBagPrefab;
    [SerializeField] private ContacktZone contacktZone;
    [SerializeField] private AutoGrid customerLine;
    
    [Header("위치")]
    [SerializeField] private Transform customerOutPoint;
    [SerializeField] private Transform paperBagPoint;
    

    private bool isPaymentAvailable = false;
    private bool isGetNextCustomer = true;

    protected override void Start()
    {
        base.Start();
        usingSlot = null;
        poolManager.SetPoolQueue(paperBagPrefab);
    }
    
    protected void Update()
    {
        if (usingSlot != null)
        {
            if (usingSlot.Customer.CheckGetPaperBag())
            {
                usingSlot.Customer = null;
                usingSlot = null;
            }
            else
            {
                // 다 받을떄까지 대기
                return;
            }
        }
        else //슬롯이 Null이면 슬롯을 뽑음
        {
            if (Count == 0) return;
            
            usingSlot = Dequeue();
            usingSlot.Customer.transform.TryGetComponent(out StackCarrier customerStack);

            if (isPaymentAvailable == false) return;
            usingSlot.Customer.SetIsGetPaperBag(true);
            GameObject tempPaperBag = poolManager.ActiveObject(paperBagPrefab, paperBagPoint.position, paperBagPoint.rotation);
        }
    }

    public override WaitingSlot Enqueue(CustomerController customerController)
    {
        var waitingSlot = GetEmptyWaitingSlot();
        if (waitingSlot == null && waitingSlots.Count < maxWaitingSlotNum)
        {
            poolManager.ActiveObject(waitPointPrefab).transform.TryGetComponent(out WaitngPoint waitPoint);
            
            waitPoint.transform.SetParent(customerLine.transform);
            customerLine.UpdateElements();
            
            WaitingSlot tempWaitingSlot = new WaitingSlot(waitPoint);
            tempWaitingSlot.Customer = customerController;
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

    public void SetPaymentAvailable(bool available)
    {
        isPaymentAvailable = available;
    }
    
}
