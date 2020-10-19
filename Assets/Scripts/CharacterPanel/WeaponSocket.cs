using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class WeaponSocket : GearSocket
{
    private float currentY;
    private float currentX;

    [SerializeField]
    private SpriteRenderer parentRenderer;

    public override void SetXAndY(float x, float y)
    {
        base.SetXAndY(x, y);

        if (currentY != y)
        {
            if (y == 1)
            {
                transform.localPosition = new Vector3(0, 0.854f, 0);
            }
            else
            {
                transform.localPosition = new Vector3(0, 0.850f, 0);
            }

            currentY = y;
        }

    }
}