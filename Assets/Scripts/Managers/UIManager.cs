using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// 绑定技能菜单
    /// </summary>
    [SerializeField]
    private CanvasGroup keybindMenu;

    /// <summary>
    /// 菜单中所有快捷键按钮的引用
    /// </summary>
    private GameObject[] keybindButtons;
    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }
	void Start ()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
        SetUseable(actionButtons[0], SpellBook.MyInstance.GetSpell("火球术"));
        SetUseable(actionButtons[1], SpellBook.MyInstance.GetSpell("寒冰箭"));
        SetUseable(actionButtons[2], SpellBook.MyInstance.GetSpell("闪电术"));
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseMenu();
        }
    }

    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);

        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);
        
        portraitFrame.sprite = target.MyPortrait;
        
        target.healthChanged += new HealthChanged(UpdateTargetFrame);
        
        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
        
    }
    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }
    /// <summary>
    /// 打开快捷键菜单
    /// </summary>
    public void OpenCloseMenu()
    {
        keybindMenu.alpha = keybindMenu.alpha > 0 ? 0 : 1;
        keybindMenu.blocksRaycasts = keybindMenu.blocksRaycasts != true;
        Time.timeScale = Time.timeScale > 0 ? 0 : 1;
    }

    /// <summary>
    /// 当快捷键改变，更新文本
    /// </summary>
    /// <param name="key">英文Key值</param>
    /// <param name="code">键位值</param>
    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    /// <summary>
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="useable"></param>
    public void SetUseable(ActionButton btn, IUseable useable)
    {
        btn.MyButton.image.sprite = useable.MyIcon;
        btn.MyButton.image.color = Color.white;
        btn.MyUseable = useable;
        

    }
}
