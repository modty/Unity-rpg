using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
 /// <summary>
    /// 当前绑定对象图片
    /// </summary>
    [SerializeField]
    private Image content;

    /// <summary>
    /// UI条上的文本
    /// </summary>
    [SerializeField]
    private Text statValue;

    /// <summary>
    /// 当前填充值
    /// </summary>
    private float currentFill;

    private float overflow;

    /// <summary>
    /// 平滑速度
    /// </summary>
    [SerializeField]
    private float lerpSpeed;

    /// <summary>
    /// 最大当前当前生命值、魔法值
    /// </summary>
    private float maxValue;

    public float MaxValue
    {
        get => maxValue;
        set
        {
            maxValue = value;
            // 计算UI填充长度
            currentFill = currentValue / MaxValue;
            if (statValue != null)
            {
                // 设置文本
                statValue.text = currentValue + " / " + MaxValue;
            }
        }
    }

    /// <summary>
    /// 当前当前生命值、魔法值
    /// </summary>
    private float currentValue;

    public bool IsFull
    {
        get { return content.fillAmount == 1; }
    }

    public float MyOverflow
    {
        get
        {
            float tmp = overflow;
            overflow = 0;
            return tmp;
        }
    }

    /// <summary>
    /// 正确更新CurrentValue值
    /// </summary>
    public float CurrentValue
    {
        get
        {

            return currentValue;
        }

        set
        {
            if (value > MaxValue)// 确保数值不上溢
            {
                overflow = value - MaxValue;
                currentValue = MaxValue;
            }
            else if (value < 0) // 确保数值不下溢
            {
                currentValue = 0;
            }
            else //数值在0~maxValue之间
            {
                currentValue = value;
            }

            // 计算UI填充长度
            currentFill = currentValue / MaxValue;
            if (statValue != null)
            {
                // 设置文本
                statValue.text = currentValue + " / " + MaxValue;
            }

        }
    }
    void Update()
    {
        HandleBar();
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="currentValue">当前UI条代表的值</param>
    /// <param name="maxValue">UI代表的最大值</param>
    public void Initialize(float currentValue, float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }

        MaxValue = maxValue;
        CurrentValue = currentValue;
        content.fillAmount = CurrentValue / MaxValue;
    }

    /// <summary>
    /// 更新UI条
    /// </summary>
    private void HandleBar()
    {
        if (currentFill != content.fillAmount) // 在数据变化后才更新UI
        {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }

    }

    public void Reset()
    {
        content.fillAmount = 0;
    }
}
