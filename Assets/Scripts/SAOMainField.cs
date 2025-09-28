using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteByName
{
    public string name;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "SAOMainField", menuName = "ScriptableObject/Main Field")]
public class SAOMainField : ScriptableObject
{
    [Header("일반")] 
    public float gameSpeed;
    public float stackSpeed;
    
    [Header("빵")] 
    public float bakeryProductionSpeed;
    public int breadPrice;
    
    [Header("고객")]
    public float customerSpeed;
    public int customerMaxStackNum;
    public int maxWantBreadNum;
    public int minWantBreadNum;
    public float customerSpawnTerm;
    public float goTableProbability;
    
    [Header("플레이어")]
    public float playerSpeed;
    public int playerMaxStackNum;
    
    [Header("오디오")]
    public float SFXVolume;
    
    [Header("스택 관련")]
    public StackAccess[] stackAccessRegister;
    
    [Header("스프라이트")]
    public Sprite[] numberSprites;
    public SpriteByName[] SpritesByName;
    

    public float putTime = 0.2f;
    public float putTerm = 1f;
    public float putCurve = 1f;
    public float putCurveMinHeight = 1f;

    public bool CheackInputAble(StackContainer requestor, StackContainer host, EStackableObjects requestorObj)
    {
        foreach (StackAccess carrier in stackAccessRegister)
        {
            if (requestor.stackType == carrier.stackContainer)
            {
                foreach (StackableObjectAccess carrierObject in carrier.stackableObjectAccess)
                {
                    if (carrierObject.stackableObjectType == requestorObj)
                    {
                        foreach (EStackableType ablcContainer in carrierObject.inputAbleContainers)
                        {
                            if(ablcContainer == host.stackType) return true;
                        }
                    }
                }
            }
        }
        
        
        return false;
    }
    
    public bool CheackOutputAble(StackContainer requestor, StackContainer host, EStackableObjects requestorObj)
    {
        foreach (StackAccess carrier in stackAccessRegister)
        {
            if (requestor.stackType == carrier.stackContainer)
            {
                foreach (StackableObjectAccess carrierObject in carrier.stackableObjectAccess)
                {
                    if (carrierObject.stackableObjectType == requestorObj)
                    {
                        foreach (EStackableType ablcContainer in carrierObject.outputAbleContainers)
                        {
                            if(ablcContainer == host.stackType) return true;
                        }
                    }
                }
            }
        }
        
        
        return false;
    }
    
    public Sprite GetNumberSpriteByInt(int number)
    {
        return numberSprites[number];
    }

    public Sprite GetSpriteByName(string name)
    {
        foreach (SpriteByName sprite in SpritesByName)
        {
            if(name == sprite.name) return sprite.sprite;
        }
        
        return null;
    }
}
