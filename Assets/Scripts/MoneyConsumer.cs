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
    private int movingMoneyCount = 0;
    
    private bool IsGivenMoney => needMoney > 0 && GameManager.Instance.money > 0;
    
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
        --needMoney;
        currentNeedMoneyText.text = needMoney.ToString();
    }

    private void PayComplte()
    {
        if (needMoney <= 0)
        {
            for(int i = 0; i < rewardObject.Length; i++) rewardObject[i].SetActive(true);
            for(int i = 0; i < offObject.Length; i++) offObject[i].SetActive(false);
        }
    }

    private void SpawnMoneyForPlayer()
    {
        if (GameManager.Instance.money > 0)
        {
            --GameManager.Instance.money;
            GameManager.Instance.moneyAmountText.text = GameManager.Instance.money.ToString();
            PoolManager.instance.ActiveObject(moneyPrefab).transform.TryGetComponent(out StackableObject stackObj);
            currentUnit.VirtualStackCarrier.GetStackObject(stackObj);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(PLAYER))
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
                    if (currentUnit.VirtualStackCarrier.IsAleadyMoveable == false && GameManager.Instance.money > 0 && currentUnit.VirtualStackCarrier.currentStackObjectType == EStackableObjects.None)
                    {
                        SpawnMoneyForPlayer();
                    }
                    else if (currentUnit.VirtualStackCarrier.IsAleadyMoveable == false && needMoney > 0 && currentUnit.VirtualStackCarrier.currentStackObjectType == EStackableObjects.Money)
                    {
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
