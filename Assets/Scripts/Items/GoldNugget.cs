﻿using System.Collections;
using System.Collections.Generic;
 using Items;
 using New;
 using UnityEngine;

[CreateAssetMenu(fileName = "GoldNugget", menuName = "Items/GoldNugget", order = 4)]
public class GoldNugget : ItemInGame
{

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>一堆碎金子。</color>");
    }

    public GoldNugget(Item item) : base(item)
    {
    }
}
