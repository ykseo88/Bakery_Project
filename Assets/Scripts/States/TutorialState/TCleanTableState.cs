using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TCleanTableState : IState
{
    private const float CameraMoveTime = 2f;
    private const float stayTime = 2f;

    private Camera _camera;
    private bool isNext = false;
    private EatTable eatTable;

    public void Enter()
    {
        _camera =  Camera.main;
        eatTable = GameManager.Instance.eatTable;
        TutorialManager.instance.player.isHandle = false;
        Vector3 origin = _camera.transform.position;
        
        _camera.transform.DOMove(TutorialManager.instance.SeeAnotherConsumePoint.position, CameraMoveTime)
            .SetDelay(stayTime);
        _camera.transform.DOMove(origin, CameraMoveTime)
            .OnComplete(() =>
                TutorialManager.instance.player.isHandle = true);



        TutorialManager.instance.player.Stay.isStaiable = true;
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}
