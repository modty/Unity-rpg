using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovedFireball : Talent
{
    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.MyInstance.GetSpell("Fireball").MyCastTime -= 0.1f;
            return true;
        }

        return false;
     
    }

    public override string GetDescription()
    {
        return string.Format("Improved Fireball\n<color=#ffd100>Reduces the casting time\nof your Fireball by 0.1 sec. </color>");
    }

}
