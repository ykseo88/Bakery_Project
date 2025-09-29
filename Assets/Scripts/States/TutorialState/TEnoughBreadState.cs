using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TEnoughBreadState : IState
{
    private const float CameraMoveTime = 2f;
    private const float stayTime = 3f;
    private Camera _camera;
    private bool isNext = false;
    private EatTable eatTable;
    public void Enter()
    {
        TutorialManager.instance.SetNextTutorial();
        _camera = Camera.main;
        eatTable = GameManager.Instance.eatTable;
        TutorialManager.instance.player.isHandle = false;
        Vector3 origin = _camera.transform.position;
        
        _camera.transform.DOMove(TutorialManager.instance.SeeTablePoint.position, CameraMoveTime)
            .SetDelay(stayTime);
        _camera.transform.DOMove(origin, CameraMoveTime)
            .OnComplete(() =>
                TutorialManager.instance.player.isHandle = true);
        
            
        
    }

    public void Update()
    {
        //if(GameManager.Instance.cashDesk.MoneyCollector.CurrentMoney == 0) TutorialManager.instance.SetNextTutorial();
        
        if(eatTable.IsOpen)
            TutorialManager.instance.ChangeState(new TCleanTableState());
    }

    public void Exit()
    {
        
    }
}
