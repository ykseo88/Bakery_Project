using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGetBreadState : IState
{
    public void Enter()
    {
        //TutorialManager.instance.SetNextTutorial();
    }

    public void Update()
    {
        if(TutorialManager.instance.player.StackCarrier.currentStackObjectType == EStackableObjects.Bread)
            TutorialManager.instance.ChangeState(new TGiveBreadState());
            
    }

    public void Exit()
    {
       
    }
}
