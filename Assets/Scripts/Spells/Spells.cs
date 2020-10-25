﻿using Assets.Scripts.Debuffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Spells : IUseable, IMoveable, IDescribable, ICastable
{
    /// <summary>
    /// 技能名
    /// </summary>
    [SerializeField]
    private string title;

    /// <summary>
    /// 技能伤害
    /// </summary>
    [SerializeField]
    private float damage;

    [SerializeField]
    private float duration;

    [SerializeField]
    private float range;

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

    [SerializeField]
    private string description;

    
    /// <summary>
    /// 技能读条颜色
    /// </summary>
    [SerializeField]
    private Color barColor;

    [SerializeField]
    private bool needsTarget;

    public Debuff MyDebuff { get; set; }

    /// <summary>
    /// 设置技能名
    /// </summary>
    public string MyTitle
    {
        get
        {
            return title;
        }
    }


    /// <summary>
    /// 获取技能伤害
    /// </summary>
    public float MyDamage
    {
        get
        {
            return Mathf.Ceil(damage);
        }
        set
        {
            damage = value;
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
        set
        {
            castTime = value;
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

    public float MyRange
    {
        get
        {
            return range;
        }

        set
        {
            range = value;
        }
    }

    public bool NeedsTarget { get => needsTarget;}
    public float MyDuration { get => duration; set => duration = value; }

    public string GetDescription()
    {
        if (!needsTarget)
        {
            return $"{title}<color=#ffd100>\n{description}\n每秒造成 {damage / MyDuration} 点伤害\n持续 {MyDuration} 秒</color>";
        }
        else
        {
            return string.Format("{0}\n花费: {1} 秒(s)\n<color=#ffd111>{2}\n造成 {3} 点伤害</color>", title, castTime, description, MyDamage);
        }


    }

    public void Use()
    {
        Player.MyInstance.CastSpell(this);
    }
}
