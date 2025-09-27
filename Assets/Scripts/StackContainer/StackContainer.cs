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
    public EStackableObjects currentStackObjectType = EStackableObjects.None;
    public StackableObject currentStackObject;
    
    public int currentStackNum = 0;
    
    public event Action isStackCountChangedEvent;

    public bool stackMovealbe = true;

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
        // 2차 베지어 곡선이 이런 종류의 호에 더 간단하고 좋은 경우가 많습니다.
        // 호를 만들기 위한 제어점을 정의합니다.
        float height = Vector3.Distance(start, end) * mainField.putCurve; // 호의 높이를 변경하려면 이 계수를 조정하세요.
        if (height < 1f) height = 1f; // 최소 높이

        Vector3 controlPoint = (start + end) * 0.5f + Vector3.up * height;

        // 2차 베지어 공식: (1-t)^2 * p0 + 2(1-t)t * p1 + t^2 * p2
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * start; // (1-t)^2 * p0
        p += 2 * u * t * controlPoint; // 2(1-t)t * p1
        p += tt * end; // t^2 * p2

        return p;
    }

    public void UpdateCurrentStackableObject()
    {
        if (currentStack.Count > 0)
        {
            currentStackObjectType = currentStack.Peek().type;
            isHasStack = true;
        }
        else
        {
            currentStackObjectType = EStackableObjects.None;
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
        currentStackObject = stackableObject;
        
        if (stackableObject.transform.parent != null)
        {
            stackableObject.autoGrid.InsertElement(stackableObject.transform);
        }
        
    }

    public StackableObject GiveStackObject()
    {
        StackableObject temp = currentStack.Pop();
        if (currentStackNum <= 0) currentStackObject = null;
        return temp;
    }

    public Stack<StackableObject> GetCurrentStack()
    {
        return currentStack;
    }

    public void ClearAndDeactivateAll()
    {
        if (autoGrid != null)
        {
            autoGrid.ClearGrid();
        }

        while (currentStack.Count > 0)
        {
            StackableObject obj = currentStack.Pop();
            if (obj != null && obj.gameObject != null) // Safety check for destroyed objects
            {
                PoolManager.instance.DeActiveObject(obj.gameObject);
            }
        }
    }
    
    public void ClearAndDeactivateOne()
    {
        if (autoGrid != null)
        {
            autoGrid.ClearGrid();
        }

        StackableObject obj = currentStack.Pop();
        if (obj != null && obj.gameObject != null) // Safety check for destroyed objects
        {
            PoolManager.instance.DeActiveObject(obj.gameObject);
        }
    }

    public void ClearStack()
    {
        foreach (StackableObject obj in currentStack)
        {
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(false);
        }
        
        currentStack.Clear();
    }

    public bool CheckCurrentStackableObjectIsNone()
    {
        if(currentStackObject == null) return false;
        return currentStackObject.isNoneStack;
    }
}
