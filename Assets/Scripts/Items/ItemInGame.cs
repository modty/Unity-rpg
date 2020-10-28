using New;
using UnityEngine;

namespace Items
{
    /// <summary>
    /// 所有物品的父类
    /// </summary>
    public class ItemInGame:IUseable
    {

        private Item item;
        private int[] inventoryPosition;

        public int[] InventoryPosition
        {
            get => inventoryPosition;
            set => inventoryPosition = value;
        }

        public Item Item
        {
            get => item;
            set => item = value;
        }

        private Sprite icon;
        private int stackCount;
        /// <summary>
        /// 物品图标，加载一次后保存，避免重复IO
        /// </summary>
        public Sprite Icon
        {
            get
            {
                if (icon == null)
                {
                    icon = Utils.LoadSpriteByIO(item.icon);
                }
                return icon;
            }
            set => icon = value;
        }

        /// <summary>
        /// 堆叠数量，如果在背包中，需要显示数量的时候调用
        /// </summary>
        public int StackCount
        {
            get
            {
                return stackCount;
            }
            set => stackCount = value;
        }

        /// <summary>
        /// 该物品的最大堆叠数
        /// </summary>
        public int MaxStackSize
        {
            get
            {
                return item.maxStackSize;
            }
        }
        public string Name
        {
            get
            {
                return item.name_cn;
            }
        }

        public int Price
        {
            get
            {
                return item.price;
            }
        }

        public long Uid
        {
            get { return item.uid; }
        }
        /// <summary>
        /// 返回特殊物品的描述
        /// </summary>
        /// <returns></returns>
        public virtual string GetDescription()
        {
            return string.Format("<color={0}>{1}</color>", "#00ff00ff", Name);
        }

        public ItemInGame(Item item)
        {
            this.Item = Utils.Clone(item);
            this.Icon = Utils.LoadSpriteByIO(item.icon);
            StackCount = 1;
        }

        public void Use()
        {
        }
    }
}

