﻿using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovedFrostbolt : Talent
{
    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.MyInstance.GetSpell("Frostbolt").MyRange += 1;
            return true;
        }

        return false;

    }

    public override string GetDescription()
    {
        return string.Format("改良的寒冰箭\n<color=#ffd100>提升 1 米寒冰箭的攻击范围。 </color>");
    }
}
