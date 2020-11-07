using System.Collections;
using System.Collections.Generic;
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
    /// 额外属性
    ///     BaseAttribute[0]：生命值
    ///     BaseAttribute[1]：魔法值
    ///     BaseAttribute[2]：攻击
    ///     BaseAttribute[3]：魔法强度
    ///     BaseAttribute[4]：护甲
    ///     BaseAttribute[5]：魔法抗性
    ///     BaseAttribute[6]：攻击速度
    ///     BaseAttribute[7]：极速
    ///     BaseAttribute[8]：暴击率
    ///     BaseAttribute[9]：移动速度
    /// </summary>
    public List<int> BaseAttribute;
    
    public CharacterState()
    {
        IsJump = false;
    }
}