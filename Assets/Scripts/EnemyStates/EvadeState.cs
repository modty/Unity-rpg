using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : IState
{
    /// <summary>
    /// 对敌人对象的引用
    /// </summary>
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.Reset();
    }

    public void Update()
    {
        // 当失去攻击目标静止后确保能够返回原本位置
        // 进行寻路
        parent.Direction = (parent.MyStartPosition - parent.transform.position).normalized;

        // 计算于原本位置的距离
        float distance = Vector2.Distance(parent.MyStartPosition, parent.transform.position);

        // 到达原本位置后静止
        if (distance <= 0.1f)
        {
            parent.ChangeState(new IdleState());
        }
    }
}
