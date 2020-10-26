using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingMaterial
{ 

    [SerializeField]
    private Item item;

    [SerializeField]
    private int count;

    public int Count
    {
        get
        {
            return count;
        }
    }

    public Item Item
    {
        get
        {
            return item;
        }
    }
}
