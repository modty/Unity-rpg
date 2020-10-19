using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField]
    protected Loot[] loot;

    public List<Drop> MyDroppedItems { get; set; }

    private bool rolled = false;

    public List<Drop> GetLoot()
    {
        if (!rolled)
        {
            MyDroppedItems = new List<Drop>();
            RollLoot();
        }

        return MyDroppedItems;
    }
    /// <summary>
    /// 根据概率添加战利品
    /// </summary>
    protected virtual void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);

            if (roll <= item.MyDropChance)
            {
                MyDroppedItems.Add(new Drop(item.MyItem, this));
            }
        }

        rolled = true;
    }
}
