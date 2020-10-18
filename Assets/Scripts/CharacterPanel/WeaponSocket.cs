using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// 32

class WeaponSocket : GearSocket
{
    private float currentY;

    [SerializeField]
    private SpriteRenderer parentRenderer;

    public override void SetXAndY(float x, float y)
    {
        base.SetXAndY(x, y);

        if (currentY != y)
        {
            if (y == 1)
            {
                spriteRenderer.sortingOrder = parentRenderer.sortingOrder - 1;
            }
            else
            {
                spriteRenderer.sortingOrder = parentRenderer.sortingOrder + 5;
            }
        }
    }
}