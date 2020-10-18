using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 36

public class Vendor : MonoBehaviour, IInteractable
{
    [SerializeField]
    private VendorItem[] items;

    [SerializeField]
    private VendorWindow vendorWindow;

    public bool IsOpen { get; set; }

    public void Interact()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            vendorWindow.CreatePages(items);
            vendorWindow.Open(this);
        }


    }

    public void StopInteract()
    {
        if (IsOpen)
        {
            IsOpen = false;
            vendorWindow.Close();
        }
    }

}
