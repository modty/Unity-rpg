using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class StunnedState : IState
{
    public void Enter(Enemy parent)
    {
        parent.Direction = Vector2.zero;
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
       
    }
}