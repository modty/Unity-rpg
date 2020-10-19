using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovedFrostbolt : Talent
{
    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.MyInstance.GetSpell("Frostbolt").MyRange += 1;
            return true;
        }

        return false;

    }

    public override string GetDescription()
    {
        return string.Format("Improved Frostbolt\n<color=#ffd100>Increases the range\nof your Frostbolt by 1. </color>");
    }
}
