using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovedThunderbolt : Talent
{
    private int percent = 5;

    public override bool Click()
    {
        if (base.Click())
        {
            Spell thunderBolt = SpellBook.MyInstance.GetSpell("Thunderbolt");

            thunderBolt.MyDamage += (thunderBolt.MyDamage / 100) * percent;
            return true;
        }

        return false;

    }

    public override string GetDescription()
    {
        return string.Format($"Improved Thunderbolt\n<color=#ffd100>Increas the damge\nof your Thunderbolt by {percent}%. </color>");
    }
}
