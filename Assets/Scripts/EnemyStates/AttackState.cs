using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    /// <summary>
    /// 敌人对象
    /// </summary>
    private Enemy parent;

    private float attackCooldown = 3;

    private float extraRange = .1f;

    /// <summary>
    /// 状态构造器
    /// </summary>
    public void Enter(Enemy parent)
    {
        this.parent = parent; 
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        // 确保只有冷却时间过后才能攻击
        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            // 初始化攻击计数器
            parent.MyAttackTime = 0;

            // 开始攻击
            parent.StartCoroutine(Attack());
        }

        if (parent.MyTarget != null) // 如果有攻击目标判断是跟随他还是攻击他
        {
            // 计算与目标之间的距离
            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

            // 如果目标太远，走向他
            if (distance >= parent.MyAttackRange+extraRange && !parent.IsAttacking)
            {
                parent.ChangeState(new FollowState());
            }
       

        }
        else// 如果距离太远，静止
        {
            parent.ChangeState(new IdleState());
        }
    }

    /// <summary>
    /// 敌人攻击角色
    /// </summary>
    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        parent.MyAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsAttacking = false;
    }

}
