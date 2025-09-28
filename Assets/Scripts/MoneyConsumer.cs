using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyConsumer : MonoBehaviour
{
    private const string PLAYER = "Player";
    private const string UNIT = "Unit";
    
    private StackInven moneyInven;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private int needMoney;
    [SerializeField] private GameObject[] rewardObject;
    [SerializeField] private GameObject[] offObject;
    [SerializeField] private TMP_Text currentNeedMoneyText;
    private UnitController currentUnit;
    private LayerMask UnitLayer;
    private int tempMoneyAmount;
    private bool isFullTempMoney;
    
    private bool IsGivenMoney => needMoney > 0 && GameManager.Instance.money > 0;
    
    public event Action PayCompleteEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.TryGetComponent(out moneyInven);
        currentNeedMoneyText.text = needMoney.ToString();
        
        UnitLayer = LayerMask.NameToLayer(UNIT);
    }

    // Update is called once per frame
    void Update()
    {
        PayComplte();
    }

    private void ConsumeMoney(StackableObject stackObj)
    {
        stackObj.IsFinishMoveEvent += UpdateMoney;
    }

    private void UpdateMoney(StackableObject finishedStackObj)
    {
        finishedStackObj.IsFinishMoveEvent -= UpdateMoney;
        //--needMoney;
        currentNeedMoneyText.text = needMoney.ToString();
    }

    private void PayComplte()
    {
        if (needMoney <= 0)
        {
            for(int i = 0; i < rewardObject.Length; i++) rewardObject[i].SetActive(true);
            for(int i = 0; i < offObject.Length; i++) offObject[i].SetActive(false);
            PayCompleteEvent?.Invoke();
        }
        
    }

    private void SpawnMoneyForPlayer()
    {
        GameManager.Instance.moneyAmountText.text = GameManager.Instance.money.ToString();
        currentNeedMoneyText.text = needMoney.ToString();
        if (GameManager.Instance.money > 0 && needMoney > 0)
        {
            GameManager.Instance.money--;
            needMoney--;
            currentNeedMoneyText.text = needMoney.ToString();
            GameManager.Instance.moneyAmountText.text = GameManager.Instance.money.ToString();
            PoolManager.instance.ActiveObject(moneyPrefab).transform.TryGetComponent(out StackableObject stackObj);
            currentUnit.VirtualStackCarrier.GetStackObject(stackObj);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == UnitLayer)
        {
            if (currentUnit == null)
            {
                other.transform.TryGetComponent(out currentUnit);
                currentUnit.VirtualStackCarrier.IsStartOneMoveEvent += ConsumeMoney;
                currentUnit.VirtualStackCarrier.SetIsContact(true);
            }
            else
            {
                if (other.gameObject.layer == UnitLayer)
                {
                    if (currentUnit.VirtualStackCarrier.IsAleadyMoveable == false && !currentUnit.VirtualStackCarrier.GetIsFullStack())
                    {
                        SpawnMoneyForPlayer();
                        currentUnit.VirtualStackCarrier.GiveObject(moneyInven, true);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.TryGetComponent(out UnitController unit))
            if (unit == currentUnit)
            {
                currentUnit.VirtualStackCarrier.IsStartOneMoveEvent -= ConsumeMoney;
                currentUnit.VirtualStackCarrier.SetIsContact(false);
                currentUnit = null;
            }
    }
}
