using Assets.Scripts.Debuffs;
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
                nextRank = $"<color=#ffffff>\n\nNext rank:\n</color><color=#ffd100>Your fireball applies a debuff\nto the target that\ndoes {tickDamage * debuff.MyDuration} damage over {debuff.MyDuration} seconds</color>\n";
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
        return $"Ignite<color=#ffd100>\nYour fireball applies a debuff\nto the target that\ndoes {debuff.MyTickDamage*debuff.MyDuration} damage over {debuff.MyDuration} seconds</color>{nextRank}";  
    }
}
