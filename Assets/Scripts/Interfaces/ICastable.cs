using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface ICastable
{
    string Title
    {
        get;
    }

    Sprite Icon
    {
        get;
    }

    float CastTime
    {
        get;
    }

    Color BarColor
    {
        get;
    }
}
