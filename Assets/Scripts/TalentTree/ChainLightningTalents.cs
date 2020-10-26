﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class ChainLightningTalents : Talent
{
    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.Instance.LearnSpell("ChainLightning");
        }
        return false;
    }

    public override string GetDescription()
    {
        return $"惊雷<color=#ffd100>\n巨大的闪电给敌人狠狠一击。</color>";
    }
}
