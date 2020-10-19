using Assets.Scripts.Debuffs;
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
        debuff.MySpeedReduction = speedReduction;
    }

    public override bool Click()
    {
        if (base.Click())
        {

            debuff.MySpeedReduction = speedReduction;

            if (MyCurrentCount < 3)
            {
                speedReduction += reductionIncrease;
                nextRank = $"<color=#ffffff>\n\nNext rank:\n</color><color=#ffd100>Your Frostbolt applies a debuff\nto the target that\nreduces the movement speed \nby {debuff.MySpeedReduction+reductionIncrease}%</color>\n";
            }
            else
            {
                nextRank = string.Empty;
            }
            SpellBook.MyInstance.GetSpell("Frostbolt").MyDebuff = debuff;
            UIManager.MyInstance.RefreshTooltip(this);
            return true;
        }

        return false;

    }

    public override string GetDescription()
    {
        return $"Permafrost<color=#ffd100>\nYour Frostbolt applies a debuff\nto the target that\nreduces the movement speed by {debuff.MySpeedReduction}%</color>{nextRank}";
    }
}
