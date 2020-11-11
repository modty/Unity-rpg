using System;

/// <summary>
/// 角色获取后的技能类
/// </summary>
[Serializable]
public class SpellInGame:Spell
{
    public SpellInGame(Spell spell) : base(
        Utils.Clone<Spell>(spell))
    {
        
    }
}
