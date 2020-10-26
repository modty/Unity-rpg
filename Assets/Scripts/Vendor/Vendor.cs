using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : NPC, IInteractable
{
    [SerializeField]
    private VendorItem[] items;

    public VendorItem[] Items
    {
        get
        {
            return items;
        }
    }
}
