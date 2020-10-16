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
        var position = parent.transform.position;
        parent.Direction = (parent.MyStartPosition - position).normalized;

        position = Vector2.MoveTowards
            (position, parent.MyStartPosition, parent.Speed * Time.deltaTime);
        parent.transform.position = position;

        // 计算于原本位置的距离
        float distance = Vector2.Distance(parent.MyStartPosition, position);

        // 到达原本位置后静止
        if (distance <= 0)
        {
            parent.ChangeState(new IdleState());
        }
    }
}
