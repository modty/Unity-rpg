using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop 
{
    public Item Item { get; set; }

    public LootTable LootTabe { get; set; }

    public Drop(Item item, LootTable lootTable)
    {
        LootTabe = lootTable;
        Item = item;
    }

    public void Remove()
    {
        LootTabe.DroppedItems.Remove(this);
    }
}
