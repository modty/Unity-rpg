using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatScript : MonoBehaviour
{
    private ControlledChaState _controlledControlledChaState;
    private CombatTextManager _combatTextManager;
    public ControlledChaState ControlledControlledChaState
    {
        get => _controlledControlledChaState;
        set => _controlledControlledChaState = value;
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
    }

    private void Start()
    {
        _combatTextManager=CombatTextManager.Instance;
        EventCenter.AddListener<AttributeChange>(Constants.EnterAttributeChange+":"+_controlledControlledChaState.Uid,UpdateAttribute);
    }

    public void Initial()
    {
        // 初始化生命值、蓝量
        healthBar.Initialize(_controlledControlledChaState.Health[0],_controlledControlledChaState.Health[1]);
        manaBar.Initialize(_controlledControlledChaState.Mana[0],_controlledControlledChaState.Mana[1]);
        // 初始化属性
        for (int i = 0; i < attributeNums.Length; i++)
        {
            if (i==4)
            {
                attributeNums[i].text = ((float)_controlledControlledChaState.BaseAttribute[i]/100).ToString("0.00");
            }
            else
            {
                attributeNums[i].text = _controlledControlledChaState.BaseAttribute[i].ToString();
            }
        }
        // 初始化面板金币
        goldNum.text = _controlledControlledChaState.GoldNum.ToString();
        // 初始化物品栏快捷键
        shortcutsScript.Initial(_controlledControlledChaState.ItemShortCuts);
    }
    public void UpdateAttribute(AttributeChange attributeChange)
    {
        int temp = 0;
        if (attributeChange.Attribute.TryGetValue(Constants.Health, out temp))
        {
            _controlledControlledChaState.Health[0] += temp;
            healthBar.CurrentValue = _controlledControlledChaState.Health[0];
            _combatTextManager.CreateText(_controlledControlledChaState.PlayerPosition, temp.ToString(), SCTTYPE.HEAL,false);
        }

        if (attributeChange.Attribute.TryGetValue(Constants.Mana, out temp))
        {
            _controlledControlledChaState.Mana[0] += temp;
            manaBar.CurrentValue = _controlledControlledChaState.Mana[0];
            _combatTextManager.CreateText(_controlledControlledChaState.PlayerPosition, temp.ToString(), SCTTYPE.MANA,false);
        }
    }
    
}
