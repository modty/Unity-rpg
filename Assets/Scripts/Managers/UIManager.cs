using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 198

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }
    /// <summary>
    /// 所有技能按钮
    /// </summary>
    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private GameObject targetFrame;
    
    private Stat healthStat;
    
    [SerializeField]
    private Image portraitFrame;

    [SerializeField]
    private GameObject tooltip;

    [SerializeField]
    private CharacterPanel charPanel;

    
    private Text tooltipText;

    
    [SerializeField]
    private RectTransform tooltipRect;

    
    /// <summary>
    /// 绑定技能菜单
    /// </summary>
    [SerializeField]
    private CanvasGroup keybindMenu;

    
    [SerializeField]
    private CanvasGroup spellBook;

    /// <summary>
    /// 菜单中所有快捷键按钮的引用
    /// </summary>
    private GameObject[] keybindButtons;
    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }
	void Start ()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
    }
	
	void Update ()
    {
        // ESC键打开快捷键菜单
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(keybindMenu);
        }
        // P键打开技能菜单
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenClose(spellBook);
        }
        // B键打开背包
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScript.MyInstance.OpenClose();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            charPanel.OpenClose();
        }
    }

    /// <summary>
    /// 显示选中敌人的界面UI
    /// </summary>
    /// <param name="target"></param>
    public void ShowTargetFrame(Enemy target)
    {
        // 设置对象有效
        targetFrame.SetActive(true);

        // 预置体复制，并初始化值
        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);
        
        // 更新目标头像
        portraitFrame.sprite = target.MyPortrait;
        
        // 绑定事件（目标血量改变、目标被移除）
        target.healthChanged += new HealthChanged(UpdateTargetFrame);
        
        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
        
    }
    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
        
    }

    /// <summary>
    /// 更新目标血量（仅数值，不更新UI）
    /// </summary>
    /// <param name="health"></param>
    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }
    
    /// <summary>
    /// 快捷键改变，更新文本
    /// </summary>
    /// <param name="key">快捷键类型</param>
    /// <param name="code">键位</param>
    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }
    /// <summary>
    /// 点击技能快捷键
    /// </summary>
    /// <param name="buttonName"></param>
    public void ClickActionButton(string buttonName)
    {
        // 唤醒当前快捷键绑定的事件
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    /// <summary>
    /// 技能菜单打开
    /// </summary>
    /// <param name="canvasGroup"></param>
    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
    
    /// <summary>
    /// 更新UI上的堆叠数量
    /// </summary>
    /// <param name="clickable">更改数量的UI位置（快捷栏、背包等）</param>
    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1) // 如果传入可点击物品是复数
        {
            // 修改相应的属性
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else // 如果只有一个，不显示数量
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = Color.white;
        }
        if (clickable.MyCount == 0) // 如果物品为空，隐藏slot
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }
    
    public void ClearStackCount(IClickable clickable)
    {
        clickable.MyStackText.color = new Color(0, 0, 0, 0);
        clickable.MyIcon.color = Color.white;
    }
    
    /// <summary>
    /// 显示提示界面
    /// </summary>
    public void ShowTooltip(Vector2 pivot,Vector3 position, IDescribable description)
    {
        tooltipRect.pivot = pivot;
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltipText.text = description.GetDescription();
    }

    /// <summary>
    /// 隐藏提示界面
    /// </summary>
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
    
    public void RefreshTooltip(IDescribable description)
    {
        tooltipText.text = description.GetDescription();
    }
}
