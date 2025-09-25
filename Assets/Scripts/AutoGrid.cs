using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAxisType
{
    X, Y, Z
}

public enum ESequenceType
{
    First, Second, Last
}

public class AutoGrid : MonoBehaviour
{
    [SerializeField] private Vector3 maxCount = new Vector3(1, 100, 1);
    [SerializeField] private Vector3 spacing;
    public Vector3 objRotation;
    [SerializeField] private Vector3 currentCount;
    
    [SerializeField] private List<Transform> elements = new List<Transform>();

    public EAxisType firstStartAxis;
    public EAxisType secondStartAxis;
    public EAxisType lastStartAxis;
    
    
    public Vector3 nextEmptyWorldPosition;
    public Vector3 nextEmptyLocalPosition = new Vector3(0, 0, 0);
    
    private StackContainer stackContainer;
    // Start is called before the first frame update
    void Start()
    {
        stackContainer = transform.root.GetComponentInChildren<StackContainer>();
        stackContainer.isStackCountChangedEvent += UpdateElements;
        UpdateElements();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEmptyPosition();
    }

    public void UpdateElements()
    {
        foreach (Transform child in transform)
        {
            if(elements.Contains(child)) elements.Add(child);
        }

        SetGrid();
    }

    public void OutElement(Transform element)
    {
        elements.Remove(element);
        SetGrid();
    }

    public void InsertElement(Transform element)
    {
        elements.Add(element);
        SetGrid();
    }

    private int GetMaxCount(ESequenceType sequenceType)
    {
        switch (sequenceType)
        {
            case ESequenceType.First:
                switch (firstStartAxis)
                {
                    case EAxisType.X:
                        return (int)maxCount.x;
                    case EAxisType.Y:
                        return (int)maxCount.y;
                    case EAxisType.Z:
                        return (int)maxCount.z;
                }
                break;
            case ESequenceType.Second:
                switch (secondStartAxis)
                {
                    case EAxisType.X:
                        return (int)maxCount.x;
                    case EAxisType.Y:
                        return (int)maxCount.y;
                    case EAxisType.Z:
                        return (int)maxCount.z;
                }
                break;
            case ESequenceType.Last:
                switch (lastStartAxis)
                {
                    case EAxisType.X:
                        return (int)maxCount.x;
                    case EAxisType.Y:
                        return (int)maxCount.y;
                    case EAxisType.Z:
                        return (int)maxCount.z;
                }
                break;
        }

        return 0;
    }

    public void SetGrid()
    {
        int firstAxis = 0;
        int secondAxis = 0;
        int lastAxis = 0;

        int firstMax = GetMaxCount(ESequenceType.First);
        int secondMax = GetMaxCount(ESequenceType.Second);
        int lastMax = GetMaxCount(ESequenceType.Last);
        
        
        
        foreach (Transform element in elements)
        {
            switch (firstStartAxis)
            {
                case EAxisType.X:
                    switch (secondStartAxis)
                    {
                        case EAxisType.Y:
                            element.localPosition = new Vector3(firstAxis * spacing.x, secondAxis * spacing.y, lastAxis * spacing.z);
                            break;
                        case EAxisType.Z:
                            element.localPosition = new Vector3(firstAxis * spacing.x, lastAxis * spacing.y, secondAxis * spacing.z);
                            break;
                    }
                    break;
                case EAxisType.Y:
                    switch (secondStartAxis)
                    {
                        case EAxisType.X:
                            element.localPosition = new Vector3(secondAxis * spacing.x, firstAxis * spacing.y, lastAxis * spacing.z);
                            break;
                        case EAxisType.Z:
                            element.localPosition = new Vector3(lastAxis * spacing.x, firstAxis * spacing.y, secondAxis * spacing.z);
                            break;
                    }
                    break;
                case EAxisType.Z:
                    switch (secondStartAxis)
                    {
                        case EAxisType.X:
                            element.localPosition = new Vector3(secondAxis * spacing.x, lastAxis * spacing.y, firstAxis * spacing.z);
                            break;
                        case EAxisType.Y:
                            element.localPosition = new Vector3(lastAxis * spacing.x, secondAxis * spacing.y, firstAxis * spacing.z);
                            break;
                    }
                    break;
            }

            if (firstAxis < firstMax)
            {
                ++firstAxis;
                if (firstAxis == firstMax && secondAxis < secondMax)
                {
                    firstAxis = 0;
                    ++secondAxis;
                    if (secondAxis == secondMax)
                    {
                        secondAxis = 0;
                        ++lastAxis;
                    }
                }
            }
            element.localRotation = Quaternion.Euler(objRotation);
        }

        
        currentCount = GetCurrentCount(firstAxis, secondAxis, lastAxis);
    }

    private Vector3 GetCurrentCount(int first, int second, int last)
    {
        Vector3 temp = Vector3.zero;
        switch (firstStartAxis)
        {
            case EAxisType.X:
                switch (secondStartAxis)
                {
                    case EAxisType.Y:
                        temp = new Vector3(first, second, last);
                        break;
                    case EAxisType.Z:
                        temp = new Vector3(first, last, second);
                        break;
                }
                break;
            case EAxisType.Y:
                switch (secondStartAxis)
                {
                    case EAxisType.X:
                        temp = new Vector3(second, first, last);
                        break;
                    case EAxisType.Z:
                        temp = new Vector3(last, first, second);
                        break;
                }
                break;
            case EAxisType.Z:
                switch (secondStartAxis)
                {
                    case EAxisType.X:
                        temp = new Vector3(second, last, first);
                        break;
                    case EAxisType.Y:
                        temp = new Vector3(last, second, first);
                        break;
                }
                break;
        }
        
        return temp;
    }

    private void UpdateEmptyPosition()
    {
        if (elements.Count > 0)
        {
            Transform tempElement = elements[^1];
            nextEmptyLocalPosition = new Vector3(spacing.x * currentCount.x, spacing.y * currentCount.y, spacing.z * currentCount.z);
        }
        else
        {
            nextEmptyLocalPosition = Vector3.zero;
        }

        nextEmptyWorldPosition = transform.TransformPoint(nextEmptyLocalPosition);
    }
    
    private void OnValidate()
    {
        if (firstStartAxis == secondStartAxis) // 겹치면 자동 보정
        {
            foreach (EAxisType value in System.Enum.GetValues(typeof(EAxisType)))
            {
                if (value != firstStartAxis)
                {
                    secondStartAxis = value;
                    break;
                }
            }
        }

        foreach (EAxisType value in System.Enum.GetValues(typeof(EAxisType)))
        {
            if (value != firstStartAxis && value != secondStartAxis)
            {
                lastStartAxis = value;
                break;
            }
        }
    }

}
