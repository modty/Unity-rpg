using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    private static SpellBook instance;

    public static SpellBook Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpellBook>();
            }

            return instance;
        }
    }

    /// <summary>
    /// 技能释放条
    /// </summary>
    [SerializeField]
    private Image castingBar;

    /// <summary>
    /// 技能读条上面的技能
    /// </summary>
    [SerializeField]
    private Text currentSpell;

    /// <summary>
    /// 技能释放条上的时间文本
    /// </summary>
    [SerializeField]
    private Text castTime;

    /// <summary>
    /// 技能释放条上的技能图标
    /// </summary>
    [SerializeField]
    private Image icon;

    /// <summary>
    /// 技能释放条浅出效果
    /// </summary>
    [SerializeField]
    private CanvasGroup canvasGroup;

    /// <summary>
    /// 所有技能
    /// </summary>
    [SerializeField]
    private Spells[] spells;

    /// <summary>
    /// 技能释放协程
    /// </summary>
    private Coroutine spellRoutine;


    /// <summary>
    /// 浅出效果协程
    /// </summary>
    private Coroutine fadeRoutine;

    [SerializeField]
    private GameObject[] obtainableSpells;


    public void Cast(ICastable castable)
    {
        //初始化填充数
        castingBar.fillAmount = 0;

        //改变技能条颜色
        castingBar.color = castable.BarColor;

        //改变技能读条文本
        currentSpell.text = castable.Title;

        //改变技能读条图标
        icon.sprite = castable.Icon;

        //开始读条
        spellRoutine = StartCoroutine(Progress(castable));

        //浅出效果
        fadeRoutine = StartCoroutine(FadeBar());
    }
    
    /// <summary>
    /// 更新技能释放条上相关信息
    /// </summary>
    /// <param name="spell">技能对象</param>
    private IEnumerator Progress(ICastable castable)
    {
        // 技能释放已经等待的时间
        float timePassed = Time.deltaTime;

        //技能释放条加载速度
        float rate = 1.0f / castable.CastTime;

        //技能释放进度
        float progress = 0.0f;

        while (progress <= 1.0)
        {
            // 根据进度更新释放条
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
            // 进度增加
            progress += rate * Time.deltaTime;

            //更新时间
            timePassed += Time.deltaTime;

            //更新文本
            castTime.text = (castable.CastTime - timePassed).ToString("F2");

            if (castable.CastTime - timePassed < 0) // 确保不小于0
            {
                castTime.text = "0.00";
            }

            yield return null;
        }

        StopCating();
    }

    /// <summary>
    /// 开始读条后添加暗淡效果
    /// </summary>
    private IEnumerator FadeBar()
    {
        // 进度条浅出速率
        float rate = 1.0f / 0.50f;

        // 进度条浅出进度
        float progress = 0.0f;

        while (progress <= 1.0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
    }

    /// <summary>
    /// 退出进度条
    /// </summary>
    public void StopCating()
    {
        // 停止浅出协程
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }
        // 停止技能协程
        if (spellRoutine != null)
        {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }

    
    public void LearnSpell(string name)
    {
        switch (name.ToLower())
        {
            case "rain of fire":
                obtainableSpells[0].SetActive(true);
                break;
            case "blizzard":
                obtainableSpells[1].SetActive(true);
                break;
            case "chainlightning":
                obtainableSpells[2].SetActive(true);
                break;

        }
    }

    public Spells GetSpell(string spellName)
    {
        Spells spell = Array.Find(spells, x => x.Title == spellName);

        return spell;
    }
}
