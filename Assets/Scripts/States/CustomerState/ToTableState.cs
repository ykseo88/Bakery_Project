using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTableState : ICustomerState
{
    private CustomerController customerController;
    public ToTableState(CustomerController customerController) => this.customerController =  customerController;
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.ToTableState);
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        
    }
}
