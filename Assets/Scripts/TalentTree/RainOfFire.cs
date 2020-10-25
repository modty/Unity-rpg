﻿using Assets.Scripts.Debuffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainOfFire : Talent
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private float damage;

    public void Start()
    {

    }

    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.MyInstance.LearnSpell("Rain Of Fire");
            
        }

        return false;

    }

    public override string GetDescription()
    {
        return $"火雨<color=#ffd100>\n在目标点召唤一场火雨。\n持续 {duration} 秒(s)，每秒造成 {damage/duration} 点伤害。 </color>";
    }
}
