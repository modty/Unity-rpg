﻿using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovedFireball : Talent
{
    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.MyInstance.GetSpell("Fireball").MyCastTime -= 0.1f;
            return true;
        }

        return false;
     
    }

    public override string GetDescription()
    {
        return string.Format("改良火球术\n<color=#ffd100>减小你的火球术 0.1 s冷却。 </color>");
    }

}
