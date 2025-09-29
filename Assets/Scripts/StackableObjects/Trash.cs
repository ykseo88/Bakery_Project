using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Trash : StackableObject
{
    private const string PLAYER = "Player";
    
    private EatTable eatTable;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(PLAYER))
        {
            SetClear();
        }
    }

    private void SetClear()
    {
        eatTable = GameManager.Instance.eatTable;
        eatTable.Clean();
        eatTable.TrashClearParticle.Play();
        eatTable.FoodSetContainer.ClearAndDeactivateAll();
    }
}
