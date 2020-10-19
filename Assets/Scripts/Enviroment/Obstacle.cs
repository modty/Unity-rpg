using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
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


    void Start()
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "WallHack")
        {
            FadeOut();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "WallHack")
        {
            FadeIn();
        }

    }

}
