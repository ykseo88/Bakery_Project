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
    
    [SerializeField] private GameObject trashPrefab;
    [SerializeField] private AutoGrid customerLine;
    [SerializeField] private MoneyCollector moneyCollector;
    
    [Header("위치")]
    [SerializeField] Transform[] stopOverPointArray;
    public Transform[] StopOverPointArray => stopOverPointArray;
    [SerializeField] private Transform foodPoint;
    public Transform FoodPoint => foodPoint;
    [SerializeField] private Transform sitPoint;
    public Transform SitPoint => sitPoint;
    [SerializeField] private StackContainer foodSetContainer;
    public StackContainer FoodSetContainer => foodSetContainer;
    [SerializeField] private Transform outPoint;
    public Transform OutPoint => outPoint;
    
    private bool isOpen = false;
    public bool IsOpen => isOpen;

    private bool isEatStart;
    
    private PayState currentPayState = PayState.CustomerWaiting;

    private int payMoney = 0;
    private int payAmount = 0;
    
    [SerializeField] MoneyConsumer moneyConsumer;
    public MoneyConsumer MoneyConsumer => moneyConsumer;

    [SerializeField] private float timePerOneFood = 1f;
    [SerializeField] private ParticleSystem trashClearParticle;
    public  ParticleSystem TrashClearParticle => trashClearParticle;
    [SerializeField] private ParticleSystem trashClearParticle2;
    public ParticleSystem TrashClearParticle2 => trashClearParticle2;

    private bool isDirty = false;
    public bool IsDirty => isDirty;
    


    protected override void Start()
    {
        base.Start();
        usingCustomer = null;
        //poolManager.SetPoolQueue(paperBagPrefab);
        isLineQueue = true;
        moneyConsumer.PayCompleteEvent += PublishOpenEvent;
        trashClearParticle.Pause();
    }
    
    protected void Update()
    {
        if (usingCustomer != null)
        {
            //Debug.Log("usigSlot 널 아님!");
            if (usingCustomer.CurrentState.GetType() == typeof(EatingState) &&
                Vector3.Distance(usingCustomer.transform.position, SitPoint.position) <
                arriveDistance)
            {
                switch (currentPayState)
                {
                    case PayState.CustomerWaiting:
                        usingCustomer.StackCarrier.IsFinishGetEvent += DonePayment;
                        currentPayState = PayState.PaymentStart;
                        break;
                    case PayState.PaymentStart:
                        StackableObject tempfood = usingCustomer.StackCarrier.GiveStackObject();
                        payAmount = usingCustomer.StackCarrier.currentStackNum;
                        payMoney = payAmount * onePerPrice;
                        usingCustomer.StackCarrier.ClearAndDeactivateAll();
                        foodSetContainer.GetStackObject(tempfood);
                        currentPayState = PayState.Processing;
                        break;
                    case PayState.Processing:
                        if (isEatStart == false)
                        {
                            isEatStart = true;
                            StartCoroutine(EatingCoroutine());
                        }
                        break;
                    case PayState.PaymentCompleted:
                        
                        isEatStart = false;
                        foodSetContainer.ClearAndDeactivateAll();
                        StackableObject tempTrash = PoolManager.instance.ActiveObject(trashPrefab).transform
                            .GetComponent<StackableObject>();
                        foodSetContainer.GetStackObject(tempTrash);
                        isDirty = true;
                        usingCustomer.Emoji.OnFloatEmoji();
                        
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
            
            PublishNewUdateEvent(usingCustomer);

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
        
        PublishUsingUpdateEvent();
    }

    private IEnumerator EatingCoroutine()
    {
        yield return new WaitForSeconds(timePerOneFood * payAmount);
        currentPayState = PayState.PaymentCompleted;
    }

    public List<Transform> GetFirstWaitPoint()
    {
        return customerLine.GetElements();
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
        UpdateQueuePoint();
        Debug.Log("결제 다함!");
        moneyConsumer.PayCompleteEvent -= PublishOpenEvent;
    }

    public void Clean()
    {
        isDirty = false;
    }
}
