using UnityEngine;

/// <summary>
/// 敌人的跟随状态
/// </summary>
class FollowState : IState
{
    /// <summary>
    /// 敌人对象的引用
    /// </summary>
    private Enemy parent;

    /// <summary>
    /// 进入状态
    /// </summary>
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    /// <summary>
    /// 退出状态
    /// </summary>
    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    /// <summary>
    /// 当在此种状态中时
    /// </summary>
    public void Update()
    {
        if (parent.MyTarget != null)// 如果有目标，保持跟随
        {
            // 找到目标的位置
            var position = parent.transform.position;
            parent.Direction = (parent.MyTarget.transform.position - position).normalized;

            // 朝目标移动
            position = Vector2.MoveTowards(position, parent.MyTarget.position, parent.Speed * Time.deltaTime);
            parent.transform.position = position;

            float distance = Vector2.Distance(parent.MyTarget.position, position);

            if (distance <= parent.MyAttackRange)
            {
                parent.ChangeState(new AttackState());
            }

        }
        if (!parent.InRange)
        {
            parent.ChangeState(new EvadeState());
        } // 失去目标，返回原位置并静止
       
    }
}
