using System;
using System.Collections.Generic;
using Items;

[Serializable]
public class Character
{
    /// <summary>
    /// 物品类别ID :3（角色）
    /// </summary>
    private long uid;
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
    /// 初始物品（初始物品必须有一个背包）
    /// </summary>
    private long[] items;
    public Character()
    {
    }

    public Character(Character character)
    {
        this.health = character.health;
    }

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

    public long Uid
    {
        get => uid;
        set => uid = value;
    }
}
