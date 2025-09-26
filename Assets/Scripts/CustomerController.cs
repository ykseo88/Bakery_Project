using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CustomerController : MonoBehaviour
{
    private const string PLAYER = "Player";
    private const string CUSTOMER = "Customer";

    public int PersonalId { get;  private set; }
    public Animator animator;
    private NavMeshAgent navMeshAgent;
    private ICustomerState currentState;
    
    private CustomerManager customerManager;
    public CustomerManager CustomerManager => customerManager;
    
    [SerializeField] private WaitingQueue showBasket;
    public ICustomerState CurrentState => currentState;
    public SAOMainField mainField; 
    
    public GameObject stateBubble;
    public SpriteRenderer currentCustomerWantMark;
    public SpriteRenderer markWithNumber;
    public TMP_Text numberText;

    public int wantBreadNum;
    public StackContainer stackContainer;
    
    public Transform centerPoint;
    
    public event Action OnImpactEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.TryGetComponent(out animator);
        transform.TryGetComponent(out navMeshAgent);
        transform.TryGetComponent(out stackContainer);
        ChangeState(new ToShowBasketState(this));
        
        wantBreadNum = Random.Range(mainField.minWantBreadNum, mainField.maxWantBreadNum + 1);
        stackContainer.maxStackNum = wantBreadNum;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("현재 상태:" + currentState);
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
        if (stackContainer.currentStackNum >= wantBreadNum) return true;
        else return false;
    }

    public void SetCustomerManager(CustomerManager cm)
    {
        customerManager = cm;
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
}
