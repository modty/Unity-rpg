﻿using Assets.Scripts.Debuffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteTalent : Talent
{

    IgniteDebuff debuff;

    private float tickDamage = 5;

    private float damageIncrease = 2;

    private string nextRank = string.Empty;

    public void Start()
    {
        debuff = new IgniteDebuff(icon);
        this.debuff.MyTickDamage = tickDamage;
    }

    public override bool Click()
    {
        if (base.Click())
        {

            debuff.MyTickDamage = tickDamage;

            if (MyCurrentCount < 3)
            {
                tickDamage += damageIncrease;
                nextRank = $"<color=#ffffff>\n\n火球术Ⅱ:\n</color><color=#ffd100> 向敌人释放一个火球\n在 {debuff.MyDuration} s内造成 {tickDamage * debuff.MyDuration} 点伤害</color>\n";
            }
            else
            {
                nextRank = string.Empty;
            }
            SpellBook.MyInstance.GetSpell("Fireball").MyDebuff = debuff;
            UIManager.MyInstance.RefreshTooltip(this);
            return true;
        }

        return false;

    }

    public override string GetDescription()
    {
        return $"火球术Ⅱ<color=#ffd100>\n 向敌人释放一个火球\n在 {debuff.MyDuration} s内造成 {debuff.MyTickDamage*debuff.MyDuration} 点伤害。</color>{nextRank}";  
    }
}
