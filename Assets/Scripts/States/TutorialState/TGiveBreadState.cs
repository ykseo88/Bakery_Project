using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGiveBreadState : IState
{
    public void Enter()
    {
        TutorialManager.instance.SetNextTutorial();
    }

    public void Update()
    {
        if(TutorialManager.instance.player.StackCarrier.currentStackObjectType == EStackableObjects.None)
            TutorialManager.instance.ChangeState(new TPayBreadState());
    }

    public void Exit()
    {
        
    }
}
