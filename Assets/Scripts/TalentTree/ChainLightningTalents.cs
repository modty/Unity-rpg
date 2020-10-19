using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class ChainLightningTalents : Talent
{
    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.MyInstance.LearnSpell("ChainLightning");
        }
        return false;
    }

    public override string GetDescription()
    {
        return $"Chain lightning<color=#ffd100>\nStrikes an enemy\nwith chain lightning</color>";
    }
}
