using UnityEngine;

namespace New
{
    /// <summary>
    /// 用户获取该消耗品后存在游戏中的对象。这样就能对已经获取到的物品进行个性修改
    /// </summary>
    public class ConsumbleInGame:Consumable
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

        public ConsumbleInGame(Consumable consumable, int[,] position,int stackCount) : base(Utils.Clone<Consumable>(consumable))
        {
            this.position = position;
            this.sprite = Utils.LoadSpriteByIO(consumable.icon);
            this.stackCount = stackCount;
        }
    }

}

