using System;
using Newtonsoft.Json;

namespace New
{
    /// <summary>
    /// 消耗品实体类，类型ID : 3
    /// </summary>
    [Serializable]
    public class Consumable:Item
    {
        /// <summary>
        /// 消耗品增加的属性
        /// attribute[0]:恢复类
        ///     attribute[0][0]:恢复生命值
        ///     attribute[0][1]:恢复魔法值
        /// </summary>
        public int[][] attribute;
        public Consumable(Consumable consumable)
        {
            // 避免重复引用
            name_cn = consumable.name_cn;
            name_us = consumable.name_us;
            description_cn = consumable.description_cn;
            attribute = consumable.attribute;
            icon = consumable.icon;
            maxStackSize = consumable.maxStackSize;
        }

        public Consumable()
        {
        }
    }

}
