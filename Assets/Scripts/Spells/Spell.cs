using System;
using UnityEngine;

[Serializable]
public class Spell:IUseable, IMoveable{
    /// <summary>
    /// 技能名
    /// </summary>
    [SerializeField]
    private string name;

    /// <summary>
    /// 技能伤害
    /// </summary>
    [SerializeField]
    private int damage;

    /// <summary>
    /// 技能图标
    /// </summary>
    [SerializeField]
    private Sprite icon;

    /// <summary>
    /// 技能移动速度
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// 技能释放速度
    /// </summary>
    [SerializeField]
    private float castTime;

    /// <summary>
    /// 技能预制件资源
    /// </summary>
    [SerializeField]
    private GameObject spellPrefab;

    /// <summary>
    /// 技能读条颜色
    /// </summary>
    [SerializeField]
    private Color barColor;

    /// <summary>
    /// 设置技能名
    /// </summary>
    public string MyName
    {
        get
        {
            return name;
        }
    }

    /// <summary>
    /// 获取技能伤害
    /// </summary>
    public int MyDamage
    {
        get
        {
            return damage;
        }

    }

    /// <summary>
    /// 获取图标
    /// </summary>
    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    public void Use()
    {
        Player.MyInstance.CastSpell(MyName);
    }

    /// <summary>
    /// 获取技能移动速度
    /// </summary>
    public float MySpeed
    {
        get
        {
            return speed;
        }
    }

    /// <summary>
    /// 获取技能释放时间
    /// </summary>
    public float MyCastTime
    {
        get
        {
            return castTime;
        }
    }

    /// <summary>
    /// 获取技能预制体资源
    /// </summary>
    public GameObject MySpellPrefab
    {
        get
        {
            return spellPrefab;
        }
    }

    /// <summary>
    /// 获取技能条颜色
    /// </summary>
    public Color MyBarColor
    {
        get
        {
            return barColor;
        }
    }
}