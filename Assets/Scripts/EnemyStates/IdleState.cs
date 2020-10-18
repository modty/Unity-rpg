using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
// 44
class IdleState : IState
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
    
        this.parent.Reset();
    }

    /// <summary>
    /// 退出状态
    /// </summary>
    public void Exit()
    {
        
    }

    /// <summary>
    /// 当在状态中
    /// </summary>
    public void Update()
    {
        // 如果有角色在附近，进入更随
        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());
        }
    }
}
