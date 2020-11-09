using System;
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
    
    
    private void Awake()
    {
        instance = this;
    }

    public void Initial()
    {
        healthBar.Initialize(controlledCharacterState.Health[0],controlledCharacterState.Health[1]);
        manaBar.Initialize(controlledCharacterState.Mana[0],controlledCharacterState.Mana[1]);
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
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            healthBar.CurrentValue -= 100;
            manaBar.CurrentValue -= 100;
        }
    }
}
