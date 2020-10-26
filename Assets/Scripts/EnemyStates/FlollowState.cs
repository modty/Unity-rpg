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

    private Vector3 offset;

    /// <summary>
    /// 进入状态
    /// </summary>
    public void Enter(Enemy parent)
    {
        Player.Instance.AddAttacker(parent);
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
        if (parent.Target != null)// 如果有目标，保持跟随
        {
            parent.Direction = ((parent.Target.transform.position+ offset) - parent.transform.position).normalized;

            float distance = Vector2.Distance(parent.Target.transform.position+offset, parent.transform.position);

            string animName = parent.SpriteRenderer.sprite.name;

            if (animName.Contains("right"))
            {
                offset = new Vector3(0.5f, 0.8f);
            }
            else if (animName.Contains("left"))
            {
                offset = new Vector3(-0.5f, 0.8f);
            }
            else if (animName.Contains("up"))
            {
                offset = new Vector3(0f, 1.2f);
            }
            else if (animName.Contains("down"))
            {
                offset = new Vector3(0, 0);
            }

            if (distance <= parent.AttackRange)
            {
                parent.ChangeState(new AttackState());
            }

        }
        if (!parent.InRange)
        {
            parent.ChangeState(new EvadeState());
        } 
       
    }
}
