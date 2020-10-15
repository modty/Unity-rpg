using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    /// <summary>
    /// 技能释放条
    /// </summary>
    [SerializeField]
    private Image castingBar;

    /// <summary>
    /// 技能释放条上的文本
    /// </summary>
    [SerializeField]
    private Text spellName;

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
    private Spell[] spells;

    /// <summary>
    /// 技能释放协程
    /// </summary>
    private Coroutine spellRoutine;

    /// <summary>
    /// 浅出效果协程
    /// </summary>
    private Coroutine fadeRoutine;

    void Start () {
		
    }
	
    void Update () {
		
    }

    public Spell CastSpell(int index)
    {
        //初始化填充数
        castingBar.fillAmount = 0;

        //改变技能条颜色
        castingBar.color = spells[index].MyBarColor;

        //改变技能读条文本
        spellName.text = spells[index].MyName;

        //改变技能读条图标
        icon.sprite = spells[index].MyIcon;

        //开始读条
        spellRoutine = StartCoroutine(Progress(index));

        //浅出效果
        fadeRoutine = StartCoroutine(FadeBar());

        //返回技能
        return spells[index];
    }
    /// <summary>
    /// 更新技能释放条上相关信息
    /// </summary>
    /// <param name="index">技能在技能数组中的位置</param>
    /// <returns></returns>
    private IEnumerator Progress(int index)
    {
        // 技能释放已经等待的时间
        float timePassed = Time.deltaTime;

        //技能释放条加载速度
        float rate = 1.0f / spells[index].MyCastTime;

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
            castTime.text = (spells[index].MyCastTime - timePassed).ToString("F2");
            Debug.Log(castTime.text);
            if (spells[index].MyCastTime - timePassed < 0) // 确保不小于0
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
}