using System;
using Newtonsoft.Json;

/// <summary>
/// 数值设定从右向左
/// 1-6：最小类型ID
/// 7-9：物品用途
///     0: 恢复型（恢复某种属性）
///         health: 生命值 
///             value: 恢复血量值
///             duration: 持续时间
///             frequency：频率 （多少时间恢复value的血量值）
///             cooling：冷却时间，使用后多少时间不能使用
///         mana：魔法值 
///             value: 恢复蓝量值
///             duration: 持续时间
///             frequency：频率 （多少时间恢复value的血量值）
///             cooling：冷却时间，使用后多少时间不能使用
///     1: 提升型（提升某种属性）
///         health（生命值）、mana（魔法值）、experience（经验值）、
///         attack（物理攻击力）、magic（魔法攻击力）、
///         armor（护甲） 、resistance（魔抗）、
///         attackSpeed（攻击速度）、cooling（冷却缩减）、
///         crit（暴击）、moveSpeed（移动速度）
///             value：提升数值
///             duration: 持续时间（-1为永久生效）
///             frequency：频率 （多少时间提升value数值）
/// 10-12：物品品质
/// 13-15：物品类别ID :2（消耗品，使用一次后消失）
///     包括：药水
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

