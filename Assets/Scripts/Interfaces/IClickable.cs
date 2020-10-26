using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

public interface IClickable
{
    Image Icon
    {
        get;
        set;
    }

    int Count
    {
        get;
    }

    Text StackText
    {
        get;
    }
}