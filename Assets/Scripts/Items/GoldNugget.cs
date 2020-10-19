using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldNugget", menuName = "Items/GoldNugget", order = 4)]
public class GoldNugget : Item
{

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>A hand full of gold nugget</color>");
    }

}
