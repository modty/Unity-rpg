using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationController
{
    /// <summary>
    /// 模拟队列
    /// </summary>
    private Sprite[] currentSprites;

    /// <summary>
    /// 角色所有动画
    /// </summary>
    private Dictionary<string, Sprite[]> allAnimation;
    
    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// 传入序列帧的位置
    /// </summary>
    /// <param name="spriteRenderer">渲染对象</param>
    /// <param name="allSpriteDir">序列帧的位置，用于初始化加载文件，格式：{X轴数目:Y轴数目:路径}</param>
    public AnimationController(SpriteRenderer spriteRenderer,Dictionary<string,string> allSpriteDir)
    {
        this.spriteRenderer = spriteRenderer;
        LoadSprites(allSpriteDir);

    }

    private void LoadSprites(Dictionary<string,string> allSpriteDir)
    {
        foreach (KeyValuePair<string,string> pair in allSpriteDir)
        {
            string[] param = pair.Value.Split(':');
            int w = Convert.ToInt32(param[0]);
            int h = Convert.ToInt32(param[1]);
            Sprite[] sprites=Utils.SplitSpriteByIO(param[3],w,h);
            allAnimation[pair.Key] = sprites;
        }
    }
    /// <summary>
    /// 增加替换的精灵
    /// </summary>
    /// <param name="showSprites">一组等待替换的图片</param>
    public void ReplaceSprites(Sprite[] showSprites)
    {
    }

    /// <summary>
    /// 供外部Update调用
    /// </summary>
    public void Show()
    {
    }
}
