using System;
using UnityEngine;

namespace New
{
    /// <summary>
    /// 所有物品父类
    /// </summary>
    [Serializable]
    public abstract class Item
    {
        public long uid;
        public string name_cn;
        public string name_us;
        public string description_cn;
        public string icon;
        public int maxStackSize;
        public int price;
    }
}