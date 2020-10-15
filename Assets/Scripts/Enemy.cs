using UnityEngine;

public class Enemy : NPC
{
    /// <summary>
    /// 血条画布
    /// </summary>
    [SerializeField]
    private CanvasGroup healthGroup;

    /// <summary>
    /// 当敌人被选中
    /// </summary>
    public override Transform Select()
    {
        //显示血条
        healthGroup.alpha = 1;

        return base.Select();
    }

    /// <summary>
    /// 当敌人被取消选中
    /// </summary>
    public override void DeSelect()
    {
        //隐藏
        healthGroup.alpha = 0;

        base.DeSelect();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        OnHealthChanged(health.MyCurrentValue);
    }
}