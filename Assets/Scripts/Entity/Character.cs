using System;

namespace New
{
    [Serializable]
    public class Character
    {
        public int health;
        public int manna;
        public int level;
        public int exp;
        public int moveSpeed;
        /// <summary>
        /// 初始物品（初始物品必须有一个背包）
        /// </summary>
        public long[] items;
        public Character()
        {
        }

        public Character(Character character)
        {
            this.health = character.health;
            this.manna = character.manna;
            this.level = character.level;
            this.exp = character.exp;
            this.moveSpeed = character.moveSpeed;
            this.items = character.items;
        }
    }
}