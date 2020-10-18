using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 13
public interface IState
{
    
    // 敌人的状态
    void Enter(Enemy parent);

    void Update();

    void Exit();
}
