﻿using System.Collections;
using System.Collections.Generic;
 using Items;
 using New;
 using UnityEngine;

[CreateAssetMenu(fileName ="HealthPotion",menuName ="Items/Potion", order =1)]
public class HealthPotion : ItemInGame, IUseable
{
    [SerializeField]
    private int health;

    public void Use()
    {
        if (Player.Instance.Health.CurrentValue < Player.Instance.Health.MaxValue)
        {
//            Remove();

            Player.Instance.GetHealth(health);
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>使用恢复 {0} 点生命值。</color>", health);
    }

    public HealthPotion(Item item) : base(item)
    {
    }
}
