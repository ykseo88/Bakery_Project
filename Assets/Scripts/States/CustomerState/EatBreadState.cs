using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatBreadState : ICustomerState
{
    private CustomerController customerController;
    
    public EatBreadState(CustomerController customerController) => this.customerController = customerController;
    
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.ToCashDeskState);
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        
    }
}
