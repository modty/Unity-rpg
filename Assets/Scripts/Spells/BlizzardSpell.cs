using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardSpell : AOESpell
{

    public override void Enter(Enemy enemy)
    {
        enemy.CurrentSpeed = enemy.Speed / 2;
        base.Enter(enemy);
    }


    public override void Exit(Enemy enemy)
    {
        enemy.CurrentSpeed = enemy.Speed;
        base.Exit(enemy);
    }
}
