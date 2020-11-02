using New;
using UnityEngine;

namespace Items
{
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

        /// <summary>
        /// 作为背包后里面包含的物品
        /// </summary>
        private ItemInGame[] containItems;

        public ItemInGame[] ContainItems
        {
            get => containItems;
            set => containItems = value;
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

        public int Capacity
        {
            get { return Item.capacity; }
            set
            {
                containItems=new ItemInGame[value];
                Item.capacity = value;
            }
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

        /// <summary>
        /// 
        /// </summary>
        private BagScript bagScript;

        public BagScript BagScript
        {
            get => bagScript;
            set => bagScript = value;
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
            containItems=new ItemInGame[item.capacity];
            StackCount = 1;
        }

        public void Use()
        {
            switch (Utils.GetItemType(item.uid))
            {
                case 0:
                    EquipmentUse();
                    break;
            }
        }


        private void EquipmentUse()
        {
            bool success=false;
            if (item is Equipment equipment)
            {
                switch (Utils.GetUseType(item.uid))
                {
                    //背包
                    case 9:
                        containItems = new ItemInGame[equipment.capacity];
                        success=BagBarScript.Instance.EquipBag(this);
                        break;
                }
                if (success)
                {
                    Debug.Log(equipment.name_cn+" 装备成功。");
                }
                else
                {
                    Debug.Log(equipment.name_cn+" 装备失败。");
                }
            }

        }
    }
}

