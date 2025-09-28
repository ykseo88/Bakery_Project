using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollector : MonoBehaviour
{
    private const string PLAYER = "Player";
    private const string UNIT = "Unit";
    
    
    [SerializeField] private GameObject MoneyPrefab;
    private StackContainer moneyInven;
    [SerializeField] private int currentMoney;
    private UnitController currentUnit;
    private StackCarrier stackCarrier;
    private LayerMask UnitLayer;
    private int movingMoneyCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.TryGetComponent(out moneyInven);
        
        UnitLayer = LayerMask.NameToLayer(UNIT);
        
        //stackCarrier.IsFinishGetEvent += GiveMoney;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetMoney(int moneyAmount)
    {
        for (int i = 0; i < moneyAmount; i++)
        {
            GameObject money = PoolManager.instance.ActiveObject(MoneyPrefab);
            money.transform.TryGetComponent(out StackableObject stackObj);
            moneyInven.GetStackObject(stackObj);
        }
        currentMoney += moneyAmount;
    }
    
    private void CollectMoney(StackableObject stackObj)
    {
        movingMoneyCount++;
        stackObj.IsFinishMoveEvent += UpdateMoney;
    }
    
    private void UpdateMoney(StackableObject finishedStackObj)
    {
        finishedStackObj.IsFinishMoveEvent -= UpdateMoney;
        --currentMoney;
        ++GameManager.Instance.money;
        
        movingMoneyCount--;
        if (movingMoneyCount == 0)
        {
            GameManager.Instance.moneyAmountText.text = GameManager.Instance.money.ToString();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        other.transform.TryGetComponent(out UnitController unit);
        if (unit.MainField.CheackOutputAble(unit.StackCarrier, moneyInven, EStackableObjects.Money) ==
            false) return;
        
        if (currentUnit == null)
        {
            other.transform.TryGetComponent(out currentUnit);
            currentUnit.VirtualStackCarrier.IsStartOneMoveEvent += CollectMoney;
            currentUnit.VirtualStackCarrier.SetIsContact(true);
        }
        else
        {
            if (other.gameObject.layer == UnitLayer)
            {
               
                if (currentUnit.VirtualStackCarrier.IsAleadyMoveable == false && !currentUnit.VirtualStackCarrier.GetIsFullStack())
                {
                    currentUnit.VirtualStackCarrier.GetObject(moneyInven, true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.TryGetComponent(out UnitController unit))
            if (unit == currentUnit)
            {
                currentUnit.VirtualStackCarrier.IsStartOneMoveEvent -= CollectMoney;
                currentUnit.VirtualStackCarrier.SetIsContact(false);
                currentUnit = null;
            }
    }

    private void SetCurrentUnit(UnitController unit)
    {
        
    }
}
