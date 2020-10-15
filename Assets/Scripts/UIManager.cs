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
    private Button[] actionButtons;

    /// <summary>
    /// 技能相对应的键盘值
    /// </summary>
    private KeyCode action1, action2, action3;
    
    [SerializeField]
    private GameObject targetFrame;
    
    private Stat healthStat;
    
    [SerializeField]
    private Image portraitFrame;

    
	void Start ()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(action1))
        {
            ActionButtonOnClick(0);
        }
        if (Input.GetKeyDown(action2))
        {
            ActionButtonOnClick(1);
        }
        if (Input.GetKeyDown(action3))
        {
            ActionButtonOnClick(2);
        }
    }

    /// <summary>
    /// 当按钮点击唤醒相应事件
    /// </summary>
    private void ActionButtonOnClick(int btnIndex)
    {
        actionButtons[btnIndex].onClick.Invoke();
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
}
