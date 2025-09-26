using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToOutState : ICustomerState
{
    private CustomerController customerController;
    
    public ToOutState(CustomerController customerController) => this.customerController =  customerController;
    
    
    public void Enter()
    {
        
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        
    }
}
