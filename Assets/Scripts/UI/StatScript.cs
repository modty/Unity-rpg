﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatScript : MonoBehaviour
{
    private CharacterState controlledCharacterState;

    public CharacterState ControlledCharacterState
    {
        get => controlledCharacterState;
        set => controlledCharacterState = value;
    }

    private static StatScript instance;
    public static StatScript Instance => instance;
    [SerializeField]
    private Text[] attributeNums;
    [SerializeField]
    private BarScript healthBar;
    [SerializeField]
    private BarScript manaBar;

    [SerializeField] private Text goldNum;

    [SerializeField]
    private ShortcutsScript shortcutsScript;
    private void Awake()
    {
        instance = this;
        EventCenter.AddListener(EventTypes.UpdatePlayerHealthManaBar,UpdateBar);
    }

    public void Initial()
    {
        // 初始化生命值、蓝量
        healthBar.Initialize(controlledCharacterState.Health[0],controlledCharacterState.Health[1]);
        manaBar.Initialize(controlledCharacterState.Mana[0],controlledCharacterState.Mana[1]);
        // 初始化属性
        for (int i = 0; i < attributeNums.Length; i++)
        {
            if (i==4)
            {
                attributeNums[i].text = ((float)controlledCharacterState.BaseAttribute[i]/100).ToString("0.00");
            }
            else
            {
                attributeNums[i].text = controlledCharacterState.BaseAttribute[i].ToString();
            }
        }
        // 初始化面板金币
        goldNum.text = controlledCharacterState.GoldNum.ToString();
        // 初始化物品栏快捷键
        shortcutsScript.ShortCutItems = controlledCharacterState.ItemShortCuts;
        shortcutsScript.Initial();
    }
    public void UpdateBar()
    {
        healthBar.CurrentValue = controlledCharacterState.Health[0];
        healthBar.MaxValue = controlledCharacterState.Health[1];
        manaBar.CurrentValue = controlledCharacterState.Mana[0];
        manaBar.MaxValue = controlledCharacterState.Mana[1];
    }
    
}