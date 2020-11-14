using System.Collections.Generic;
using Items;
using UnityEngine;

public class CharacterInGame:Character
{
    // 角色位置
    private Vector2 playerPosition;
    // 角色是否跳跃
    private bool isJump;
    // 角色朝向
    private Vector2 moveVec;
    
    private Vector2 mousePosition;
    public Vector2 PlayerPosition
    {
        get => playerPosition;
        set => playerPosition = value;
    }

    public bool IsJump
    {
        get => isJump;
        set => isJump = value;
    }

    public Vector2 MoveVec
    {
        get => moveVec;
        set => moveVec = value;
    }

    public Vector2 MousePosition
    {
        get => mousePosition;
        set => mousePosition = value;
    }
    
    public CharacterInGame(Character character):base(Utils.Clone<Character>(character))
    {
        
    }

    public CharacterInGame()
    {
    }
}
