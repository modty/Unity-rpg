using Assets.Scripts.Debuffs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
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
    private CanvasGroup[] menus;


    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Image portraitFrame;

    [SerializeField]
    private GameObject tooltip;

    [SerializeField]
    private CharacterPanel charPanel;

    private Text tooltipText;

    [SerializeField]
    private RectTransform tooltipRect;

    [SerializeField]
    private TargetDebuff targetDebuffPrefab;

    [SerializeField]
    private Transform targetDebuffsTransform;

    private List<TargetDebuff> targetDebuffs = new List<TargetDebuff>();

    /// <summary>
    /// 菜单中所有快捷键按钮的引用
    /// </summary>
    private GameObject[] keybindButtons;

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(menus[0]);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenClose(menus[1]);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScript.Instance.OpenClose();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenClose(menus[2]);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OpenClose(menus[3]);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenClose(menus[6]);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            OpenClose(menus[7]);
        }

    }

    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);

        healthStat.Initialize(target.Health.CurrentValue, target.Health.MaxValue);

        portraitFrame.sprite = target.Portrait;

        levelText.text = target.Level.ToString();

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

        if (target.Level >= Player.Instance.Level + 5)
        {
            levelText.color = Color.red;
        }
        else if (target.Level == Player.Instance.Level + 3 || target.Level == Player.Instance.Level + 4)
        {
            levelText.color = new Color32(255, 124, 0, 255);
        }
        else if (target.Level >= Player.Instance.Level -2 && target.Level <= Player.Instance.Level+2)
        {
            levelText.color = Color.yellow;
        }
        else if (target.Level <= Player.Instance.Level-3 && target.Level > XPManager.CalculateGrayLevel())
        {
            levelText.color = Color.green;
        }
        else
        {
            levelText.color = Color.grey;
        }

        for (int i = 0; i < targetDebuffs.Count; i++)
        {
            Destroy(targetDebuffs[i].gameObject);
        }

        targetDebuffs.Clear();

        foreach (Debuff debuff in target.Debuffs)
        {
            TargetDebuff td = Instantiate(targetDebuffPrefab, targetDebuffsTransform);
            td.Initialize(debuff);
            targetDebuffs.Add(td);
        }
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
        healthStat.CurrentValue = health;
    }

    public void AddDebuffToTargetFrame(Debuff debuff)
    {
        if (targetFrame.activeSelf && debuff.Character == Player.Instance.Target)
        {
            TargetDebuff td = Instantiate(targetDebuffPrefab, targetDebuffsTransform);
            td.Initialize(debuff);
            targetDebuffs.Add(td);
        }
    }

    public void RemoveDebuff(Debuff debuff)
    {
        if (targetFrame.activeSelf && debuff.Character == Player.Instance.Target)
        {
            TargetDebuff td = targetDebuffs.Find(x => x.Debuff.Name == debuff.Name);

            targetDebuffs.Remove(td);
            Destroy(td.gameObject);
        }
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
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).Button.onClick.Invoke();
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

    public void OpenSingle(CanvasGroup canvasGroup)
    {
        foreach (CanvasGroup canvas in menus)
        {
            CloseSingle(canvas);
        }

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void CloseSingle(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha  = 0;
        canvasGroup.blocksRaycasts = false;

    }

    /// <summary>
    /// 更新UI上的堆叠数量
    /// </summary>
    /// <param name="clickable">更改数量的UI位置（快捷栏、背包等）</param>
    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.Count > 1)  // 如果传入可点击物品是复数
        {
            clickable.StackText.text = clickable.Count.ToString();
            clickable.StackText.enabled = true;
            clickable.Icon.enabled = true;
        }
        else // 如果只有一个，不显示数量
        {
            clickable.StackText.enabled = false;
            clickable.Icon.enabled = true;
        }
        if (clickable.Count == 0)  // 如果物品为空，隐藏slot
        {
            clickable.StackText.enabled = false;
            clickable.Icon.enabled = false;
        }
    }

    public void ClearStackCount(IClickable clickable)
    {
        clickable.StackText.enabled = false;
        clickable.Icon.enabled = true;
    }

    
    /// <summary>
    /// 显示提示界面
    /// </summary>
    public void ShowTooltip(Vector2 pivot, Vector3 position, IDescribable description)
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
