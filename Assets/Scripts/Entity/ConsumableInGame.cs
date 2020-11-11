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
    public void Use()
    {
        switch (Utils.GetUseType(uid))
        {
            case 0:
                UseConsumableInGame0();
                break;
        }
    }
    /// <summary>
    /// 使用恢复型消耗品
    /// </summary>
    public void UseConsumableInGame0()
    {
        if (attribute.ContainsKey(Constants.Health))
        {
            int[] health = Player.Instance.CharacterState.Health;
            Dictionary<string,int> dictionary = attribute[Constants.Health];
            int[] param={0,0,-1,1,0,0};
            if (dictionary.TryGetValue(Constants.BaseValue, out param[0]))
            {
                health[0] += param[0];
                // 立刻恢复生命值
                EventCenter.Broadcast(EventTypes.UpdatePlayerHealthManaBar);
            }
            // 消耗品是持续性消耗品
            if (dictionary.TryGetValue(Constants.Duration,out param[2]) && param[2]!=-1)
            {
                if (dictionary.TryGetValue(Constants.Frequency, out param[3]) && param[3] > 0)
                {
                    param[5] = param[2] / param[3];
                    if (dictionary.TryGetValue(Constants.Value, out param[1]))
                    {
                        Timer _testTimer = null;
                        _testTimer = Timer.Register(
                            duration: 1f,
                            () =>
                            {
                                health[0] += param[1];
                                EventCenter.Broadcast(EventTypes.UpdatePlayerHealthManaBar);
                                param[4]++;
                                if (param[4] >= param[5])
                                {
                                    _testTimer.Cancel();
                                }
                            },
                            isLooped: true);
                    }
                }
                
            }
        }
        if (attribute.ContainsKey(Constants.Mana))
        {
            int[] mana = Player.Instance.CharacterState.Mana;
            Dictionary<string,int> dictionary = attribute[Constants.Mana];
            int[] param={0,0,-1,1,0,0};
            if (dictionary.TryGetValue(Constants.BaseValue, out param[0]))
            {
                mana[0] += param[0];
                // 立刻恢复生命值
                EventCenter.Broadcast(EventTypes.UpdatePlayerHealthManaBar);
            }
            // 消耗品是持续性消耗品
            if (dictionary.TryGetValue(Constants.Duration,out param[2]) && param[2]!=-1)
            {
                if (dictionary.TryGetValue(Constants.Frequency, out param[3]) && param[3] > 0)
                {
                    param[5] = param[2] / param[3];
                    if (dictionary.TryGetValue(Constants.Value, out param[1]))
                    {
                        Timer _testTimer = null;
                        _testTimer = Timer.Register(
                            duration: 1f,
                            () =>
                            {
                                mana[0] += param[1];
                                EventCenter.Broadcast(EventTypes.UpdatePlayerHealthManaBar);
                                param[4]++;
                                if (param[4] >= param[5])
                                {
                                    _testTimer.Cancel();
                                }
                            },
                            isLooped: true);
                    }
                }
                
            }
        }
    }

    public bool CanUse()
    {
        return true;
    }
}


