using System.Collections;
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
                nextRank = $"<color=#ffffff>\n\nNext rank:\n</color><color=#ffd100>Your Thunderbolt has a {debuff.ProcChance + procIncrease}% chance\nto stun the target for {debuff.MyDuration} second(s)</color>\n";
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
        return $"Thundershock<color=#ffd100>\nYour Thunderbolt has a {debuff.ProcChance}% chance\nto stun the target for {debuff.MyDuration} second(s)</color>{nextRank}";
    }
}
