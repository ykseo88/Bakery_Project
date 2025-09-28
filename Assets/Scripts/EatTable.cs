using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatTable : WaitingQueue
{
    private const float arriveDistance = 0.5f;
    private const float rightAngle = 90f;
    
    private StackContainer showBasketContainer;
    private CustomerController usingCustomer;
    public CustomerController UsingCustomer => usingCustomer;
    
    [SerializeField] private GameObject paperBagPrefab;
    [SerializeField] private AutoGrid customerLine;
    [SerializeField] private MoneyCollector moneyCollector;
    
    [Header("위치")]
    [SerializeField] Transform[] stopOverPointArray;
    public Transform[] StopOverPointArray => stopOverPointArray;
    [SerializeField] private Transform foodPoint;
    [SerializeField] private Transform sitPoint;
    public Transform SitPoint => sitPoint;
    [SerializeField] private StackContainer foodSetContainer;
    
    private bool isStartBreadInsert = false;
    private bool isOpen = false;
    public bool IsOpen => isOpen;
    
    private PayState currentPayState = PayState.CustomerWaiting;
    private PaperBag currentPaperBag;

    private int payMoney = 0;
    
    [SerializeField] MoneyConsumer moneyConsumer;
    public MoneyConsumer MoneyConsumer => moneyConsumer;
    


    protected override void Start()
    {
        base.Start();
        usingCustomer = null;
        //poolManager.SetPoolQueue(paperBagPrefab);
        isLineQueue = true;
        moneyConsumer.PayCompleteEvent += PublishOpenEvent;
    }
    
    protected void Update()
    {
        if (usingCustomer != null)
        {
            //Debug.Log("usigSlot 널 아님!");
            if (usingCustomer.CurrentState.GetType() == typeof(WaitTableStete) &&
                Vector3.Distance(usingCustomer.transform.position, customerLine.transform.position) <
                arriveDistance)
            {
                switch (currentPayState)
                {
                    case PayState.CustomerWaiting:
                        if (usingCustomer.CurrentState.GetType() == typeof(EatingState))
                        {
                            usingCustomer.StackCarrier.IsFinishGiveEvent += StartPacking;
                            usingCustomer.StackCarrier.IsFinishGetEvent += DonePayment;
                            usingCustomer.StackCarrier.SetIsContact(true);
                            currentPayState = PayState.PaymentStart;
                        }
                        break;
                    case PayState.PaymentStart:
                        
                        StackableObject tempfood = usingCustomer.StackCarrier.GiveStackObject();
                        usingCustomer.StackCarrier.ClearAndDeactivateAll();
                        foodSetContainer.GetStackObject(tempfood);
                        
                        payMoney = usingCustomer.StackCarrier.currentStackNum;
                        
                        currentPayState = PayState.BreadInserting;
                        break;
                    case PayState.BreadInserting:
                        if (currentPaperBag.IsGetAllBread)
                        {
                            currentPaperBag.stackContainer.ClearAndDeactivateAll();
                            usingCustomer.StackCarrier.GetObject(foodSetContainer);
                        }
                        break;
                    case PayState.PaymentCompleted:
                        usingCustomer.SetIsGetPaperBag(true);
                        usingCustomer.StackCarrier.autoGrid.objRotation.y -= rightAngle;
                        usingCustomer.StackCarrier.IsFinishGiveEvent -= StartPacking;
                        usingCustomer.StackCarrier.IsFinishGetEvent -= DonePayment;
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
            PoolManager.instance.ActiveObject(waitPointPrefab).transform.TryGetComponent(out WaitngPoint waitPoint);
            
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
    }

    public List<Transform> GetFirstWaitPoint()
    {
        return customerLine.GetElements();
    }

    private void StartPacking(EStackableObjects type)
    {
        usingCustomer.StackCarrier.autoGrid.objRotation.y += rightAngle;
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

    public override void PublishOpenEvent()
    {
        base.PublishOpenEvent();
        isOpen = true;
        Debug.Log("결제 다함!");
        moneyConsumer.PayCompleteEvent -= PublishOpenEvent;
    }
}
