using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingState : IUnitState
{
    private CustomerController customerController;
    
    public EatingState(CustomerController customerController) => this.customerController = customerController;
    
    
    
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.EatingState);
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        
    }
}
