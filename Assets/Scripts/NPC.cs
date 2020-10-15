using UnityEngine;

public class NPC : Character
{

    public virtual void DeSelect()
    {

    }

    public virtual Transform Select()
    {
        return hitBox;
    }
}