using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ContacktZone : MonoBehaviour
{
    protected const string PLAYER = "Player";
    
    [SerializeField] protected float contackSizeMultiple;
    [SerializeField] protected float effectTime;
    
    
    protected bool isContack = false;
    private Vector3 originSize;
    private Vector3 contackSize;

    protected void Start()
    {
        originSize = transform.localScale;
        contackSize = transform.localScale * contackSizeMultiple;
    }


    protected virtual void ToggleContack(bool contack)
    {
        isContack = contack;
        OnEffectContack(isContack);
    }

    private void OnEffectContack(bool isBigger)
    {
        
        switch (isBigger)
        {
            case true:
                transform.DOScale(contackSize, effectTime);
                break;
            case false:
                transform.DOScale(originSize, effectTime);
                break;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER))
        {
            ToggleContack(true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER))
        {
            ToggleContack(false);
        }
    }
}