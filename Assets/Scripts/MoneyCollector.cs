using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollector : MonoBehaviour
{
    [SerializeField] private GameObject MoneyPrefab;
    private StackInven moneyInven;
    [SerializeField] private int currentMoney;
    [SerializeField] private StackCarrier stackCarrier;
    private AutoGrid autoGrid;
    // Start is called before the first frame update
    void Start()
    {
        transform.TryGetComponent(out moneyInven);
        transform.TryGetComponent(out autoGrid);
        stackCarrier.IsFinishGetEvent += GiveMoney;
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

    private void GiveMoney(EStackableObjects type)
    {
        GameManager.Instance.money += currentMoney;
        currentMoney = 0;
    }
}
