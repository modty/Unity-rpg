using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

// 24

public interface IClickable
{
    Image MyIcon
    {
        get;
        set;
    }

    int MyCount
    {
        get;
    }

    Text MyStackText
    {
        get;
    }
}