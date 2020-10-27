using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Loot
{
    [FormerlySerializedAs("item")] [SerializeField]
    private ItemInGame itemInGame;

    [SerializeField]
    private float dropChance;

    public ItemInGame ItemInGame
    {
        get
        {
            return itemInGame;
        }
    }

    public float DropChance
    {
        get
        {
            return dropChance;
        }
    }
}
