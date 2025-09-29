using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TGetMoneyState : IState
{
    
    
    
    public void Enter()
    {
        TutorialManager.instance.SetNextTutorial();
    }

    public void Update()
    {
        if (GameManager.Instance.money >= GameManager.Instance.moneyConsumer.NeedMoney)
        {
            TutorialManager.instance.ChangeState(new TEnoughBreadState());
        }
    }

    public void Exit()
    {
        
    }
}
