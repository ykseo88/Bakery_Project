using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public Transform centerPoint;
    public Transform spawnPoint;
    
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private SAOMainField mainField;
    public ShowBasket showBasket;
    public WaitingQueue CashDesk;

    private bool isSpawnable = true;
    public int allowSpawnNum;
    private int maxSpawnableNum;
    
    
    public List<CustomerController> allCustomers = new List<CustomerController>();

    private void Start()
    {
        poolManager.SetPoolQueue(customerPrefab);
        allowSpawnNum = showBasket.maxWaitingSlotNum;
        maxSpawnableNum = showBasket.maxWaitingSlotNum;
    }

    private void Update()
    {
        if (allowSpawnNum > 0) SpawnCustomer();
    }
    
    public void SpawnCustomer()
    {
        if (isSpawnable)
        {
            isSpawnable = false;
            GameObject newCustomer = poolManager.ActiveObject(customerPrefab, spawnPoint.position, spawnPoint.rotation);
            newCustomer.transform.SetParent(null);
            newCustomer.transform.TryGetComponent(out CustomerController customerController);
            customerController.SetCustomerManager(this);
            allCustomers.Add(customerController);
            allowSpawnNum--;
            StartCoroutine(CoolSpawnTerm());
        }
    }

    private IEnumerator CoolSpawnTerm()
    {
        yield return new WaitForSeconds(mainField.customerSpawnTerm);
        isSpawnable = true;
    }
}
