using System.Collections.Generic;

namespace New
{
    /// <summary>
    /// 技能相关(有冲突，命名为Spells，后面会更改为Spell)
    /// </summary>
    public class Spell:Item
    {

        /// <summary>
        /// 技能释放时间(ms),castTime[]
        ///     castTime[0] : 释放时间，-1立即释放
        ///     castTime[1] : 升级后增加或减少属性，-0.1：即每次升级减少0.1秒释放时间
        /// </summary>
        public int[] castTime;

        /// <summary>
        /// 直接伤害，释放后立即造成伤害，采用数组，后续添加伤害的属性，每种属性伤害必须为二元值，第二个值为升级改变的相对值
        /// </summary>
        public int[][] directDamage;

        /// <summary>
        /// 持续伤害。为空则没有持续伤害
        ///     continuousDamage[0]：每次伤害
        ///     continuousDamage[1]：持续时间（ms）
        ///     continuousDamage[2]: 每秒频率
        ///  continuousDamage=[10,10000,2] : 每次造成10点伤害，每秒2次，持续10秒，共200点伤害
        /// </summary>
        public int[][] continuousDamage;

        /// <summary>
        /// 技能冷却时间，二元组（ms）
        /// </summary>
        public int[] cooldown;

        /// <summary>
        /// 下一等级技能，-1：没有下一等级
        /// </summary>
        public long nextLevel;

        /// <summary>
        /// 前一等级技能，-1：没有前一等级
        /// </summary>
        public long preLevel;

        /// <summary>
        /// <summary>
        /// 释放技能后对敌人添加的Buff。为空不添加Buff
        /// </summary>
        public int[] target_buff;

        /// <summary>
        /// 释放技能后对自己添加的Buff。为空不添加Buff
        /// </summary>
        public int[] self_buff;

        /// <summary>
        /// 技能可作用对象类型集合。为空可对任意对象释放
        /// </summary>
        public int[] target_type;

        /// 技能图标，用于UI
        /// </summary>
        public string icon;

        /// <summary>
        /// 技能图片（释放展示用）
        /// </summary>
        public string spellImage;

        /// <summary>
        /// 释放类型（直接释放：直接调用技能图片移动向目标）
        /// </summary>
        public int releaseType;

        /// <summary>
        /// 技能当前等级（spellLevel[0]）和最大等级spellLevel[1]
        /// </summary>
        public int[] spellLevel;
    }
}
