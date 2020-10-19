using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ArmorType {Head, Shoulders, Chest, Hands, Legs ,Feet, MainHand, Offhand, TwoHand }

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]
public class Armor : Item
{
    [SerializeField]
    private ArmorType armorType;

    [SerializeField]
    private int intellect;

    [SerializeField]
    private int strength;

    [SerializeField]
    private int stamina;

    [SerializeField]
    private AnimationClip[] animationClips;

    internal ArmorType MyArmorType
    {
        get
        {
            return armorType;
        }
    }

    public AnimationClip[] MyAnimationClips
    {
        get
        {
            return animationClips;
        }
    }

    public override string GetDescription()
    {
        string stats = string.Empty;

        if (intellect > 0 )
        {
            stats += string.Format("\n +{0} intellect", intellect);
        }
        if (strength > 0)
        {
            stats += string.Format("\n +{0} strength", strength);
        }
        if (stamina > 0)
        {
            stats += string.Format("\n +{0} stamina", stamina);
        }

        return base.GetDescription() +stats;
    }

    public void Equip()
    {
        CharacterPanel.MyInstance.EquipArmor(this);
    }
}
