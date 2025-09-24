using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public enum EStackableType
{
    Customer,
    Player,
    ShowBasket,
    CashDesk,
    Table,
    Oven,
    Consume
}

public class StackContainer : MonoBehaviour
{
    
    protected Stack<StackableObject> currentStack = new Stack<StackableObject>();
    protected SAOMainField mainField;
    [SerializeField] protected float putTime = 0.2f;
    [SerializeField] protected float putTerm = 0.1f;
    public int maxStackNum = 10;
    public Transform stackPoint;
    public AutoGrid autoGrid;

    public bool isHasStack;
    public EStackableType stackType;
    //public List<EStackableObjects> stackObjects = new List<EStackableObjects>();
    public EStackableObjects currentStackObject = EStackableObjects.None;
    
    public int currentStackNum = 0;
    
    public event Action isStackCountChangedEvent;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        mainField = GameManager.Instance.mainField;
        if(stackPoint !=  null) stackPoint.transform.TryGetComponent(out autoGrid);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentStackNum = currentStack.Count;
        UpdateCurrentStackableObject();
    }
    
    public Vector3 GetBezierPoint(Vector3 start, Vector3 end, float t)
    {
        // 두 점의 중간 위치
        Vector3 mid = (start + end) * 0.5f;

        // 높이 차이
        float heightDiff = end.y - start.y;

        // 제어점 두 개 생성
        // Y축 방향으로 곡선을 만들기 위해 mid를 기준으로 offset 적용
        Vector3 control1 = mid;
        Vector3 control2 = mid;

        // offset 크기 (거리에 비례, 높이 차이에 따라 분배)
        float distance = Vector3.Distance(start, end);
        float yOffset = Mathf.Max(distance * 0.25f, 0.1f); // 곡선 강도

        // 끝점이 더 높을수록 제어점이 끝점 쪽으로 몰리게
        float bias = Mathf.InverseLerp(-distance, distance, heightDiff);

        control1.y += yOffset * (1f - bias); // 시작점 쪽 제어점
        control2.y += yOffset * bias;        // 끝점 쪽 제어점

        // 3차 베지어 공식
        return Mathf.Pow(1 - t, 3) * start +
               3 * Mathf.Pow(1 - t, 2) * t * control1 +
               3 * (1 - t) * Mathf.Pow(t, 2) * control2 +
               Mathf.Pow(t, 3) * end;
    }

    public void UpdateCurrentStackableObject()
    {
        if (currentStack.Count > 0)
        {
            currentStackObject = currentStack.Peek().type;
            isHasStack = true;
        }
        else
        {
            currentStackObject = EStackableObjects.None;
            isHasStack = false;
        }
    }

    public bool GetIsFullStack()
    {
        return currentStack.Count >= maxStackNum;
    }

    public bool GetIsZeroStack()
    {
        return currentStack.Count <= 0;
    }
    
    public void GetStackObject(StackableObject stackableObject)
    {
        currentStack.Push(stackableObject);
        if(stackPoint !=null){stackableObject.transform.SetParent(stackPoint.transform);}
        stackableObject.CheckParentGrid();
        
        if (stackableObject.transform.parent != null)
        {
            stackableObject.autoGrid.InsertElement(stackableObject.transform);
        }
        
    }

    public StackableObject GiveStackObject()
    {
        return currentStack.Pop();
    }

    public Stack<StackableObject> GetCurrentStack()
    {
        return currentStack;
    }
}
