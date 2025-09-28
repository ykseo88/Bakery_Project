using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum ECustomerStates
{
    ToShowBasketState,
    WaitBreadState,
    ToCashDeskState,
    WaitPayState,
    ToOutState,
    ToTableState,
    GetTableState,
    EatingState,
    WaitTableStete,
    NoneState
}

public class CustomerController : UnitController
{
    
    
    private const string PLAYER = "Player";
    private const string CUSTOMER = "Customer";
    

    public int PersonalId { get;  private set; }
    private Animator animator;
    public Animator Animator => animator;
    private NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    
    
    private CustomerManager customerManager;
    public CustomerManager CustomerManager=> customerManager;
    
    [SerializeField] private WaitingQueue showBasket;
    public SAOMainField mainField; 
    
    public GameObject stateBubble;
    public SpriteRenderer currentCustomerWantMark;
    public SpriteRenderer markWithNumber;
    public TMP_Text numberText;

    public int wantBreadNum;
    private bool isHasPaperBag = false;
    
    private bool isArrivedQueuePoint = false;
    
    private float orginStackAngle;
    
    [SerializeField] private ECustomerStates debugCurrentState;
    
    public event Action OnImpactEvent;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        wantBreadNum = Random.Range(mainField.minWantBreadNum, mainField.maxWantBreadNum + 1);
        stackCarrier.maxStackNum = wantBreadNum;
    }

    private void OnEnable()
    {
        transform.TryGetComponent(out animator);
        transform.TryGetComponent(out navMeshAgent);
        transform.TryGetComponent(out stackCarrier);
    }

    // Update is called once per frame

    public bool CheckFullGetBread()
    {
        if (stackCarrier.currentStackNum >= wantBreadNum) return true;
        else return false;
    }

    public bool CheckGetPaperBag()
    {
        return isHasPaperBag;
    }

    public void SetCustomerManager(CustomerManager cm)
    {
        customerManager = cm;
    }

    public void SetIsGetPaperBag(bool isPaperBag)
    {
        isHasPaperBag = isPaperBag;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(CUSTOMER) || other.gameObject.CompareTag(PLAYER))
        {
            OnImpactEvent?.Invoke();
        }
    }

    public void SetPersonalId(int personalId)
    {
        PersonalId = personalId;
    }

    public void SetIsArrivedQueuePoint(bool isArrived)
    {
        isArrivedQueuePoint = isArrived;
    }
    
    public bool CheckIsArrivedQueuePoint()
    {
        return isArrivedQueuePoint;
    }

    public void SetDebugCurrentState(ECustomerStates state)
    {
        debugCurrentState = state;
    }

    public void Reset()
    {
        ChangeState(new ToShowBasketState(this));
        
        stackCarrier.ClearStack();
        wantBreadNum = Random.Range(mainField.minWantBreadNum, mainField.maxWantBreadNum + 1);
        stackCarrier.maxStackNum = wantBreadNum;
        //stackCarrier.autoGrid.objRotation.y = orginStackAngle;
        isHasPaperBag = false; 
        isArrivedQueuePoint = false;
        stateBubble.SetActive(false);
        currentCustomerWantMark.enabled = false;
        markWithNumber.enabled = false;
        numberText.enabled = false;
    }
}
