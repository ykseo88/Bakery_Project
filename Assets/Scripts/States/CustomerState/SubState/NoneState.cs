using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneState : IUnitState
{
    private CustomerController customerController;
    public NoneState(CustomerController customerController) => this.customerController = customerController;
    public void Enter()
    {
        customerController.SetDebugCurrentState(ECustomerStates.NoneState);
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        
    }
}
