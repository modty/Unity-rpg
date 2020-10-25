using System;

namespace New
{
    [Serializable]
    public class Equipment:Item
    {

        /// <summary>
        /// 装备属性
        ///     attribute[0]:智力
        ///     attribute[1]:力量
        ///     attribute[2]:耐力
        /// </summary>
        public int[] attribute;
        /// <summary>
        /// 价格
        /// </summary>
        public int price;

        /// <summary>
        /// 背包容量。其他装备一般不会调用，不排除以后会调用
        /// </summary>
        public int capacity;
        public Equipment(Equipment equipment)
        {
            name_cn = equipment.name_cn;
            this.uid = equipment.uid;
            this.attribute = equipment.attribute;
            description_cn = equipment.description_cn;
            this.icon = equipment.icon;
            this.maxStackSize = equipment.maxStackSize;
            this.price = equipment.price;
            capacity = equipment.capacity;
        }
    }
}
