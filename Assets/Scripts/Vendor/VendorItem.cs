using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class VendorItem
{
    [FormerlySerializedAs("item")] [SerializeField]
    private ItemInGame itemInGame;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private bool unlimited;

    public ItemInGame ItemInGame
    {
        get
        {
            return itemInGame;
        }
    }

    public int Quantity
    {
        get
        {
            return quantity;
        }

        set
        {
            quantity = value;
        }
    }

    public bool Unlimited
    {
        get
        {
            return unlimited;
        }
    }
}
