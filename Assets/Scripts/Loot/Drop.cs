using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop 
{
    public Item MyItem { get; set; }

    public LootTable MyLootTabe { get; set; }

    public Drop(Item item, LootTable lootTable)
    {
        MyLootTabe = lootTable;
        MyItem = item;
    }

    public void Remove()
    {
        MyLootTabe.MyDroppedItems.Remove(this);
    }
}
