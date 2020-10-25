﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardTalent : Talent
{
    [SerializeField]
    private float duration;

    public void Start()
    {

    }

    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.MyInstance.LearnSpell("Blizzard");

        }

        return false;

    }

    public override string GetDescription()
    {
        return $"暴雪<color=#ffd100>\n在目标地点释放一个暴雪\n造成50%减速。 </color>";
    }
}
