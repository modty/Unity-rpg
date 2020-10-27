using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField]
    protected Loot[] loot;

    public List<Drop> DroppedItems { get; set; }

    private bool rolled = false;

    public List<Drop> GetLoot()
    {
        if (!rolled)
        {
            DroppedItems = new List<Drop>();
            RollLoot();
        }

        return DroppedItems;
    }
    /// <summary>
    /// 根据概率添加战利品
    /// </summary>
    protected virtual void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);

            if (roll <= item.DropChance)
            {
                DroppedItems.Add(new Drop(item.ItemInGame, this));
            }
        }

        rolled = true;
    }
}
