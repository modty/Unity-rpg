﻿using Assets.Scripts.Debuffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermafrostTalent : Talent
{
    PermafrostDebuff debuff;

    private float speedReduction = 20;

    private float reductionIncrease = 10;

    private string nextRank = string.Empty;

    public void Start()
    {
        debuff = new PermafrostDebuff(icon);
        debuff.SpeedReduction = speedReduction;
    }

    public override bool Click()
    {
        if (base.Click())
        {

            debuff.SpeedReduction = speedReduction;

            if (CurrentCount < 3)
            {
                speedReduction += reductionIncrease;
                nextRank = $"<color=#ffffff>\n\n下一等级:\n</color><color=#ffd100>你的寒冰箭能使敌人减速\n减少敌人{debuff.SpeedReduction+reductionIncrease}% 的移动速度</color>\n";
            }
            else
            {
                nextRank = string.Empty;
            }
            SpellBook.Instance.GetSpell("Frostbolt").Debuff = debuff;
            UIManager.Instance.RefreshTooltip(this);
            return true;
        }

        return false;

    }

    public override string GetDescription()
    {
        return $"寒冰箭Ⅱ<color=#ffd100>\n你的寒冰箭能使敌人减速\n减少敌人 {debuff.SpeedReduction}%  的移动速度</color>{nextRank}";
    }
}
