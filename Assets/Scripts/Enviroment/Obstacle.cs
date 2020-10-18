using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 70
public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    /// <summary>
    /// 碰撞对象的精灵素材
    /// </summary>
    public SpriteRenderer MySpriteRenderer { get; set; }
    
    /// <summary>
    /// 没有虚化之前的颜色
    /// </summary>
    private Color defaultColor;

    /// <summary>
    /// 虚化后的颜色
    /// </summary>
    private Color fadedColor;
    
    /// <summary>
    /// 重载对比方法，通过对比sortingOrder来比较对象，进行对象排序
    /// </summary>
    /// <param name="other"></param>
    public int CompareTo(Obstacle other)
    {
        if (MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1;
        }
        else if (MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
        {
            return -1;
        }

        return 0;
    }

    void Start ()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();

        // 初始化透明度为0.7f
        defaultColor = MySpriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.7f;
	}
    /// <summary>
    /// 透明度修改
    /// </summary>
    public void FadeOut()
    {
        MySpriteRenderer.color = fadedColor;
    }

    /// <summary>
    /// 透明度还原
    /// </summary>
    public void FadeIn()
    {
        MySpriteRenderer.color = defaultColor;
    }
}
