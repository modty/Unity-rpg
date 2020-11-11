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
    private Vector2 playerPosition;
    // 角色是否跳跃
    private bool isJump;
    // 角色朝向
    private Vector2 moveVec;
    
    private Vector2 mousePosition;

    /// <summary>
    /// 生命值。[0]:当前生命值，[1]:最大生命值
    /// </summary>
    private int[] health;

    /// <summary>
    /// 魔法值
    /// </summary>
    private int[] mana;
        
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
    private List<int> baseAttribute;

    /// <summary>
    /// 角色金币数量
    /// </summary>
    private int goldNum;

    /// <summary>
    /// 角色经验
    /// </summary>
    private int experience;

    /// <summary>
    /// 角色等级
    /// </summary>
    private int level;

    public Vector2 PlayerPosition
    {
        get => playerPosition;
        set => playerPosition = value;
    }

    public bool IsJump
    {
        get => isJump;
        set => isJump = value;
    }

    public Vector2 MoveVec
    {
        get => moveVec;
        set => moveVec = value;
    }

    public Vector2 MousePosition
    {
        get => mousePosition;
        set => mousePosition = value;
    }

    public int[] Health
    {
        get => health;
        set { health = value; }
    }

    public int[] Mana
    {
        get => mana;
        set 
        {
            if (mana == null)
            {
                mana = value;
            }
            else
            {
                if (value[0] != mana[0] || value[1] != mana[1])
                {
                    mana = value;
                    EventCenter.Broadcast(EventTypes.UpdatePlayerHealthManaBar);
                }
            }
        }
    }

    public List<int> BaseAttribute
    {
        get => baseAttribute;
        set => baseAttribute = value;
    }

    public int GoldNum
    {
        get => goldNum;
        set => goldNum = value;
    }

    public int Experience
    {
        get => experience;
        set => experience = value;
    }

    public int Level
    {
        get => level;
        set => level = value;
    }

    /// <summary>
    /// 角色快捷键
    /// </summary>
    public Dictionary<int, ItemInGame> ItemShortCuts;
    
    public CharacterState()
    {
    }
}