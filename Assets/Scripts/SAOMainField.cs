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
    
    
}
