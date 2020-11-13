using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户获取该消耗品后存在游戏中的对象。这样就能对已经获取到的物品进行个性修改
/// </summary>
[Serializable]
public class ConsumableInGame:Consumable,Useable
{
    public ConsumableInGame(Consumable consumable) : base(Utils.Clone<Consumable>(consumable))
    {
    }
    /// <summary>
    /// 按照消耗品类别进行相应使用操作
    ///     0: 数值型消耗品，使用后增加
    /// </summary>
    public bool Use()
    {
        bool success = false;
        switch (Utils.GetUseType(uid))
        {
            case 0:
                success=UseConsumableInGame0();
                break;
        }
        return success;
    }
    /// <summary>
    /// 使用恢复型消耗品
    /// </summary>
    public bool UseConsumableInGame0()
    {
        if (attribute.ContainsKey(Constants.Health))
        {
            Utils.ItemAttributeHelper(attribute,Constants.Health,uid,Player.Instance.Uid(),Constants.EnterAttributeChange);
        }
        if (attribute.ContainsKey(Constants.Mana))
        {
            Utils.ItemAttributeHelper(attribute,Constants.Mana,uid,Player.Instance.Uid(),Constants.EnterAttributeChange);
        }

        return true;
    }

    public bool CanUse()
    {
        return true;
    }
    
    
}


