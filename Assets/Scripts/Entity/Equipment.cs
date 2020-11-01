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
        /// 动画序列
        /// </summary>
        public string animation;
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

        public Equipment()
        {
        }
    }
}
