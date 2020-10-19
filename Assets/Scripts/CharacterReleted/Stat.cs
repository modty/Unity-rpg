using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 148
public class Stat : MonoBehaviour
{

    // 当前绑定对象图片
    private Image content;
    /// <summary>
    /// UI条上的文本
    /// </summary>
    [SerializeField]
    private Text statValue;
    // 当前填充值
    private float currentFill;

    // 平滑速度
    [SerializeField]
    private float lerpSpeed;

    // 最大当前当前生命值、魔法值
    public float MyMaxValue { get; set; }

    private float overflow;

    // 当前当前生命值、魔法值
    private float currentValue;

    // 正确更新CurrentValue值
    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if (value > MyMaxValue)// 确保数值不上溢
            {
                overflow = value - MyMaxValue;
                currentValue = MyMaxValue;
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
            currentFill = currentValue / MyMaxValue;
            if (statValue != null)
            {
                // 设置文本
                statValue.text = currentValue + " / " + MyMaxValue;
            }
        }
    }

    public bool IsFull
    {
        get
        {
            return content.fillAmount == 1;
        }
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



    void Start()
    {
        content = GetComponent<Image>();
    }

    void Update()
    {
        
        HandleBar();
    }

    // 初始化数据
    // currentValue ：当前UI条代表的值
    // maxValue ： UI代表的最大值
    public void Initialize(float currentValue, float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    // 更新UI条
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
