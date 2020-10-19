using System.Collections;
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
        return $"Blizzard<color=#ffd100>\nCreates a Blizzard\non a target location\nthat reduces movement speed by 50% </color>";
    }
}
