﻿using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovedThunderbolt : Talent
{
    private int percent = 5;

    public override bool Click()
    {
        if (base.Click())
        {
            Spells thunderBolt = SpellBook.Instance.GetSpell("Thunderbolt");

            thunderBolt.Damage += (thunderBolt.Damage / 100) * percent;
            return true;
        }

        return false;

    }

    public override string GetDescription()
    {
        return string.Format($"改良的闪电箭\n<color=#ffd100>提升你的闪电箭 {percent}% 的伤害。 </color>");
    }
}
