using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// 所有技能按钮
    /// </summary>
    [SerializeField]
    private Button[] actionButtons;

    /// <summary>
    /// 技能相对应的键盘值
    /// </summary>
    private KeyCode action1, action2, action3;

	void Start ()
    {
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

}
