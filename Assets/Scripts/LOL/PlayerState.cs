using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色的状态类
/// </summary>
public class PlayerState
{
    private static PlayerState _instance;
    // 角色位置
    public Vector2 PlayerPosition;
    // 角色是否跳跃
    public bool IsJump;
    // 角色朝向
    public Vector2 MoveVec;
    
    public static PlayerState Instance => _instance ?? (_instance = new PlayerState());

    public Vector2 MousePosition;


    PlayerState()
    {
        IsJump = false;
    }
}