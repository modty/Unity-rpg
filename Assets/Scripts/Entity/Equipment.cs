using System;

/// <summary>
/// 数值设定从右向左
/// 1-6：最小类型ID
/// 7-9：物品用途
///     0: 头盔
///     1: 护肩
///     2：铠甲
///     3：手套
///     4：裤子
///     5：鞋子
///     6：武器
///     7：盾牌
/// 10-12：物品品质
/// 13-15：物品类别ID :0（装备）
/// </summary>
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
        uid = equipment.uid;
        attribute = equipment.attribute;
        description_cn = equipment.description_cn;
        icon = equipment.icon;
        maxStackSize = equipment.maxStackSize;
        price = equipment.price;
        capacity = equipment.capacity;
    }

    public Equipment()
    {
    }
}
