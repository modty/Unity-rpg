using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
/// <summary>
/// 角色的状态类
/// </summary>
public class CharacterState
{
    // 角色位置
    public Vector2 PlayerPosition;
    // 角色是否跳跃
    public bool IsJump;
    // 角色朝向
    public Vector2 MoveVec;
    
    public Vector2 MousePosition;

    /// <summary>
    /// 生命值。[0]:当前生命值，[1]:最大生命值
    /// </summary>
    public int[] Health;

    /// <summary>
    /// 魔法值
    /// </summary>
    public int[] Mana;
        
    /// <summary>
    /// 额外属性
    ///     BaseAttribute[0]：攻击
    ///     BaseAttribute[1]：魔法强度
    ///     BaseAttribute[2]：护甲
    ///     BaseAttribute[3]：魔法抗性
    ///     BaseAttribute[4]：攻击速度
    ///     BaseAttribute[5]：极速
    ///     BaseAttribute[6]：暴击率
    ///     BaseAttribute[7]：移动速度
    /// </summary>
    public List<int> BaseAttribute;

    /// <summary>
    /// 角色金币数量
    /// </summary>
    public int GoldNum;

    /// <summary>
    /// 角色经验
    /// </summary>
    public int Experience;

    /// <summary>
    /// 角色等级
    /// </summary>
    public int Level;

    /// <summary>
    /// 角色快捷键
    /// </summary>
    public Dictionary<int, ItemInGame> ItemShortCuts;
    
    public CharacterState()
    {
    }
}