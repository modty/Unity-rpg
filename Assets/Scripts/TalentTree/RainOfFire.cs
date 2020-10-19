using Assets.Scripts.Debuffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainOfFire : Talent
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private float damage;

    public void Start()
    {

    }

    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.MyInstance.LearnSpell("Rain Of Fire");
            
        }

        return false;

    }

    public override string GetDescription()
    {
        return $"Rain Of Fire<color=#ffd100>\nCreates a rain of fire\non a target location\nthat does {damage/duration} damage \nevery second for {duration} seconds</color>";
    }
}
