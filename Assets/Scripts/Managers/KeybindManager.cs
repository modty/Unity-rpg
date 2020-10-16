using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    /// <summary>
    /// 单例模式
    /// </summary>
    private static KeybindManager instance;

    /// <summary>
    /// 单例访问（构造器）
    /// </summary>
    public static KeybindManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
            }

            return instance;
        }
    }

    /// <summary>
    /// 为所有移动键绑定的字典
    /// </summary>
    public Dictionary<string, KeyCode> Keybinds { get; private set; }

    /// <summary>
    /// 为所有动作键绑定的字典
    /// </summary>
    public Dictionary<string, KeyCode> ActionBinds { get; private set; }

    /// <summary>
    /// 试图改变键的名称
    /// </summary>
    private string bindName;

    void Start ()
    {
        Keybinds = new Dictionary<string, KeyCode>();

        ActionBinds = new Dictionary<string, KeyCode>();

        //绑定初始键位
        BindKey("UP", KeyCode.W);
        BindKey("LEFT", KeyCode.A);
        BindKey("DOWN", KeyCode.S);
        BindKey("RIGHT", KeyCode.D);

        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);
    }

    /// <summary>
    /// 绑定特殊键位
    /// </summary>
    /// <param name="key">绑定的字符串</param>
    /// <param name="keyBind">绑定的键位值</param>
    public void BindKey(string key, KeyCode keyBind)
    {
        // 为快捷键定义默认字典
        Dictionary<string, KeyCode> currentDictionary = Keybinds;

        if (key.Contains("ACT")) // 如果绑定动作键位，使用动作字典
        {
            currentDictionary = ActionBinds;
        }
        if (!currentDictionary.ContainsKey(key))// 判断主键是否已经包含
        {
            //新增键位
            currentDictionary.Add(key, keyBind);

            //更新UI内容
            UIManager.MyInstance.UpdateKeyText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind))
        {
            //搜寻旧的快捷键
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            //取消旧的快捷键
            currentDictionary[myKey] = KeyCode.None;
            UIManager.MyInstance.UpdateKeyText(key, KeyCode.None);
        }

        // 确保新的快捷键已经绑定
        currentDictionary[key] = keyBind;
        UIManager.MyInstance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    /// <summary>
    /// 根据传入字符串更改键位
    /// </summary>
    /// <param name="bindName"></param>
    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }


    private void OnGUI()
    {
        if (bindName != string.Empty)// 检测是否保存快捷键
        {
            Event e = Event.current; // 监听事件

            if (e.isKey) // 如果传入的是Key,改变快捷键
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
