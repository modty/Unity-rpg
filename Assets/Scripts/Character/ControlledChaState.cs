using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
/// <summary>
/// 角色的状态类
/// </summary>
public class ControlledChaState:CharacterInGame
{
    
    /// <summary>
    /// 角色快捷键(操控角色含有快捷键)
    /// </summary>
    public Dictionary<int, ItemInGame> ItemShortCuts;

}