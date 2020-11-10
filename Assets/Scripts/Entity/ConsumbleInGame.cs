using UnityEngine;

/// <summary>
/// 用户获取该消耗品后存在游戏中的对象。这样就能对已经获取到的物品进行个性修改
/// </summary>
public class ConsumbleInGame:Consumable,Useable
{
    public ConsumbleInGame(Consumable consumable) : base(Utils.Clone<Consumable>(consumable))
    {
    }
    /// <summary>
    /// 按照消耗品类别进行相应使用操作
    ///     0: 数值型消耗品，使用后增加
    /// </summary>
    public void Use()
    {
        switch (Utils.GetUseType(uid))
        {
            
        }
    }

    public bool CanUse()
    {
        return true;
    }
}


