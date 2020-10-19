using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface ICastable
{
    string MyTitle
    {
        get;
    }

    Sprite MyIcon
    {
        get;
    }

    float MyCastTime
    {
        get;
    }

    Color MyBarColor
    {
        get;
    }
}
