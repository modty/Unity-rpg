using UnityEngine;

namespace New
{
    public class EquipmentInGame : Equipment
    {
        /// <summary>
        /// 在背包中的位置
        ///     position[i][0]:背包ID
        ///     position[i][1]:在该背包中的位置
        /// 堆叠物品肯定是一样的，所以应该能满足需求
        /// </summary>
        private int[,] position;

        /// <summary>
        /// 背包中的堆叠数目
        /// </summary>
        private int stackCount;

        /// <summary>
        /// 加载后的图标，避免重复IO
        /// </summary>
        private Sprite sprite;

        public EquipmentInGame(Equipment equipment, int[,] position, int stackCount) : base(
            Utils.Clone<Equipment>(equipment))
        {
            this.position = position;
            this.sprite = Utils.LoadSpriteByIO(equipment.icon);
            this.stackCount = stackCount;
        }

    }
}
