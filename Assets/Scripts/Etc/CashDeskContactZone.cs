using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashDeskContactZone : ContacktZone
{
    [SerializeField] private CashDesk cashDesk;
    

    protected override void ToggleContack(bool isContack)
    {
        base.ToggleContack(isContack);
        cashDesk.SetPaymentAvailable(isContack);
    }
}
