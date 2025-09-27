using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashDesk : WaitingQueue
{
    enum PayState
    {
        CustomerWaiting,
        PaymentStart,
        BreadInserting,
        PaymentCompleted,
        
    }
    
    private const float arriveDistance = 0.5f;
    private const float rightAngle = 90f;
    
    private StackContainer showBasketContainer;
    private CustomerController usingCustomer;
    
    [SerializeField] private GameObject paperBagPrefab;
    [SerializeField] private ContacktZone contacktZone;
    [SerializeField] private AutoGrid customerLine;
    [SerializeField] private MoneyCollector moneyCollector;
    
    [Header("위치")]
    public Transform customerOutPoint;
    [SerializeField] private Transform paperBagPoint;
    [SerializeField] private StackContainer paperBagContainer;
    

    private bool isPaymentAvailable = false;
    private bool isStartBreadInsert = false;
    
    private PayState currentPayState = PayState.CustomerWaiting;
    private PaperBag currentPaperBag;

    private int payMoney = 0;
    
    public event Action PaymentCompletedEvent;

    protected override void Start()
    {
        base.Start();
        usingCustomer = null;
        poolManager.SetPoolQueue(paperBagPrefab);
    }
    
    protected void Update()
    {
        if (usingCustomer != null)
        {
            //Debug.Log("usigSlot 널 아님!");
            if (isPaymentAvailable && usingCustomer.CurrentState.GetType() == typeof(WaitPayState) &&
                Vector3.Distance(usingCustomer.transform.position, customerLine.transform.position) <
                arriveDistance)
            {
                switch (currentPayState)
                {
                    case PayState.CustomerWaiting:
                        usingCustomer.stackCarrier.IsFinishGiveEvent += StartPacking;
                        usingCustomer.stackCarrier.IsFinishGetEvent += DonePayment;
                        GameObject tempPaperBag = poolManager.ActiveObject(paperBagPrefab, paperBagPoint.position,
                            paperBagPoint.rotation);
                        tempPaperBag.transform.TryGetComponent(out currentPaperBag);
                        tempPaperBag.transform.TryGetComponent(out StackableObject stackableObject);
                        paperBagContainer.GetStackObject(stackableObject);
                        currentPayState = PayState.PaymentStart;
                        break;
                    case PayState.PaymentStart:
                        payMoney = usingCustomer.stackCarrier.currentStackNum;
                        usingCustomer.stackCarrier.GiveObject(currentPaperBag.transform.position,
                            currentPaperBag.stackContainer, true);
                        currentPayState = PayState.BreadInserting;
                        break;
                    case PayState.BreadInserting:
                        if (currentPaperBag.GetisGetAllBread())
                        {
                            currentPaperBag.stackContainer.ClearAndDeactivateAll();
                            usingCustomer.stackCarrier.GetObject(usingCustomer.stackCarrier.stackPoint.position, paperBagContainer);
                        }
                        break;
                    case PayState.PaymentCompleted:
                        usingCustomer.SetIsGetPaperBag(true);
                        usingCustomer.stackCarrier.autoGrid.objRotation.y -= rightAngle;
                        usingCustomer.stackCarrier.IsFinishGiveEvent -= StartPacking;
                        usingCustomer.stackCarrier.IsFinishGetEvent -= DonePayment;
                        GetWaitingSlotOrNullByCustomer(usingCustomer).Customer = null;
                        usingCustomer = null;
                        moneyCollector.GetMoney(payMoney);
                        break;
                }
            }
            else
            {
                return;
            } 
                
        }
        else //슬롯이 Null이면 슬롯을 뽑음
        {
            //Debug.Log("usigSlot 널!");
            if (Count == 0) return;
            
            usingCustomer = Dequeue();

            if (currentPayState == PayState.PaymentCompleted) UpdateQueuePoint();
            
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
            
            WaitingSlot tempWaitingSlot = new WaitingSlot(waitPoint, customerController);
            waitingSlots.Add(tempWaitingSlot);
            //waitQueue.Enqueue(tempWaitingSlot);
            waitQueue.Enqueue(customerController);
            return tempWaitingSlot;
        }
        else
        {
            waitingSlot.Customer = customerController;
            waitQueue.Enqueue(customerController);
            return waitingSlot;
        }
    }

    public void UpdateQueuePoint()
    {
        for (int i = 1; i < waitingSlots.Count; i++)
        {
            waitingSlots[i - 1].Customer = waitingSlots[i].Customer;
        }

        waitingSlots[^1].Customer = null;
        currentPayState = PayState.CustomerWaiting;
        
        PaymentCompletedEvent?.Invoke();
    }

    public void SetPaymentAvailable(bool available)
    {
        isPaymentAvailable = available;
    }

    public List<Transform> GetFirstWaitPoint()
    {
        return customerLine.GetElements();
    }

    private void StartPacking(EStackableObjects type)
    {
        usingCustomer.stackCarrier.autoGrid.objRotation.y += rightAngle;
        PaperBagClose();
    }

    private void PaperBagClose()
    {
        currentPaperBag.SetClose();
    }

    private void DonePayment(EStackableObjects type)
    {
        if (type == EStackableObjects.PaperBag)
        {
            currentPayState = PayState.PaymentCompleted;
        }
    }

    private void SetOff(StackableObject Bread)
    {
        Bread.gameObject.SetActive(false);
    }

    public void SetWaitingState()
    {

        if (currentPayState == PayState.PaymentCompleted)
        {
            currentPayState = PayState.CustomerWaiting;
        }
    }
    
    
}
