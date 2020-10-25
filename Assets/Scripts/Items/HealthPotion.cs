﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HealthPotion",menuName ="Items/Potion", order =1)]
public class HealthPotion : Item, IUseable
{
    [SerializeField]
    private int health;

    public void Use()
    {
        if (Player.MyInstance.MyHealth.MyCurrentValue < Player.MyInstance.MyHealth.MyMaxValue)
        {
            Remove();

            Player.MyInstance.GetHealth(health);
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>使用恢复 {0} 点生命值。</color>", health);
    }

}
