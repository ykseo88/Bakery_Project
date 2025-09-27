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
    EatBreadState,
}

public class CustomerController : MonoBehaviour
{
    
    
    private const string PLAYER = "Player";
    private const string CUSTOMER = "Customer";
    

    public int PersonalId { get;  private set; }
    public Animator animator;
    private NavMeshAgent navMeshAgent;
    private ICustomerState currentState;
    
    private CustomerManager customerManager;
    public CustomerManager CustomerManager=> customerManager;
    
    [SerializeField] private WaitingQueue showBasket;
    public ICustomerState CurrentState => currentState;
    public SAOMainField mainField; 
    
    public GameObject stateBubble;
    public SpriteRenderer currentCustomerWantMark;
    public SpriteRenderer markWithNumber;
    public TMP_Text numberText;

    public int wantBreadNum;
    private bool isHasPaperBag = false;
    public StackCarrier stackCarrier;
    
    private bool isArrivedQueuePoint = false;
    
    private float orginStackAngle;
    
    [SerializeField] private ECustomerStates debugCurrentState;
    
    public event Action OnImpactEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        //ChangeState(new ToShowBasketState(this));
        
        wantBreadNum = Random.Range(mainField.minWantBreadNum, mainField.maxWantBreadNum + 1);
        stackCarrier.maxStackNum = wantBreadNum;
        orginStackAngle = stackCarrier.autoGrid.objRotation.y;
    }

    private void OnEnable()
    {
        transform.TryGetComponent(out animator);
        transform.TryGetComponent(out navMeshAgent);
        transform.TryGetComponent(out stackCarrier);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();
    }
    
    public void ChangeState(ICustomerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

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
        stackCarrier.autoGrid.objRotation.y = orginStackAngle;
        isHasPaperBag = false; 
        isArrivedQueuePoint = false;
    }
}
