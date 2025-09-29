using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    protected StackCarrier stackCarrier;
    [SerializeField] protected StackCarrier virtualStackCarrier;
    [SerializeField] protected SAOMainField mainField;
    public SAOMainField MainField => mainField;
    
    protected IState currentState;
    public IState CurrentState => currentState;

    protected virtual void Start()
    {
        transform.TryGetComponent(out stackCarrier);
    }

    protected virtual void Update()
    {
        currentState.Update();
    }
    
    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
    
    public StackCarrier StackCarrier => stackCarrier;
    public StackCarrier VirtualStackCarrier => virtualStackCarrier;
}
