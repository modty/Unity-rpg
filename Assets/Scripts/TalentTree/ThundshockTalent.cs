﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThundshockTalent : Talent
{
    ThundshockDebuff debuff;

    private string nextRank = string.Empty;

    private float procChance;
    private float procIncrease = 5;

    public void Start()
    {
       debuff = new ThundshockDebuff(icon);
        procChance = 5;
        debuff.ProcChance = procChance;

    }

    public override bool Click()
    {
        if (base.Click())
        {

            debuff.ProcChance = procChance;

            if (MyCurrentCount < 3)
            {
                procChance += procIncrease;
                nextRank = $"<color=#ffffff>\n\n下一等级:\n</color><color=#ffd100>向敌人释放闪电箭\n有 {debuff.ProcChance + procIncrease}% 的几率对敌人造成 {debuff.MyDuration} 秒(s)的眩晕。</color>\n";
            }
            else
            {
                nextRank = string.Empty;
            }
            SpellBook.MyInstance.GetSpell("Thunderbolt").MyDebuff = debuff;
            UIManager.MyInstance.RefreshTooltip(this);
            return true;
        }

        return false;

    }

    public override string GetDescription()
    {
        return $"闪电箭Ⅱ<color=#ffd100>\n向敌人释放闪电箭\n有 {debuff.ProcChance}% 的几率对敌人造成 {debuff.MyDuration} 秒(s)的眩晕。</color>{nextRank}";
    }
}
