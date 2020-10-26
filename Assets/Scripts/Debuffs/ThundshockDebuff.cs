using Assets.Scripts.Debuffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThundshockDebuff : Debuff
{
    public float SpeedReduction { get; set; }

    public override string Name => "Thundershock";

    public ThundshockDebuff(Image icon) : base(icon)
    {
        Duration = 1;
    }

    public override Debuff Clone()
    {
        ThundshockDebuff clone = (ThundshockDebuff)this.MemberwiseClone();

        return clone;
    }

    public override void Apply(Character character)
    {
        (character as Enemy).ChangeState(new StunnedState());
        base.Apply(character);
    }

    public override void Remove()
    {
        (Character as Enemy).ChangeState(new PathState());
        base.Remove();
    }
}
