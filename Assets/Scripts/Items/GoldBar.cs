﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldBar", menuName = "Items/GoldBar", order = 3)]
public class GoldBar : Item
{

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>一根根闪亮的金条!</color>");
    }

}
