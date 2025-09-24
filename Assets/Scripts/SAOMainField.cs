using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    [Header("플레이어")]
    public float playerSpeed;
    public int playerMaxStackNum;
    
    [Header("오디오")]
    public float SFXVolume;
    
    [Header("스택 관련")]
    public StackAccess[] stackAccessRegister;

    public float putTime = 0.2f;
    public float putTerm = 1f;

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
}
