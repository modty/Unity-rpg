using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class CraftingMaterial
{ 

    [FormerlySerializedAs("item")] [SerializeField]
    private ItemInGame itemInGame;

    [SerializeField]
    private int count;

    public int Count
    {
        get
        {
            return count;
        }
    }

    public ItemInGame ItemInGame
    {
        get
        {
            return itemInGame;
        }
    }
}
