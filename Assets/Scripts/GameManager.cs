using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SAOMainField mainField;

    public int money = 0;
    
    public TMP_Text moneyAmountText;
    
    public EatTable eatTable;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
