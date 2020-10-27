using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class Drop 
{
    public ItemInGame ItemInGame { get; set; }

    public LootTable LootTabe { get; set; }

    public Drop(ItemInGame itemInGame, LootTable lootTable)
    {
        LootTabe = lootTable;
        ItemInGame = itemInGame;
    }

    public void Remove()
    {
        LootTabe.DroppedItems.Remove(this);
    }
}
