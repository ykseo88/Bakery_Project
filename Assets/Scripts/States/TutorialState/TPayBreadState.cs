using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPayBreadState : IState
{
    private CashDesk cashDesk;
    
    public void Enter()
    {
        TutorialManager.instance.SetNextTutorial();
        cashDesk = GameManager.Instance.cashDesk;
        cashDesk.DonePaymentEvent += PayClear;
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        
    }

    private void PayClear()
    {
        cashDesk.DonePaymentEvent -= PayClear;
        TutorialManager.instance.ChangeState(new TGetMoneyState());
    }
}
