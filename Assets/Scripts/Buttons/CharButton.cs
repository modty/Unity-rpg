using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    private ArmorType armoryType;

    private Armor equippedArmor;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private GearSocket gearSocket;

    public Armor MyEquippedArmor
    {
        get
        {
            return equippedArmor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.Instance.Moveable is Armor)
            {
                Armor tmp = (Armor)HandScript.Instance.Moveable;

                if (tmp.ArmorType == armoryType)
                {
                    EquipArmor(tmp);
                }

//                UIManager.Instance.RefreshTooltip(tmp);
            }
            else if(HandScript.Instance.Moveable == null && MyEquippedArmor != null)
            {
              
//                HandScript.Instance.TakeMoveable(MyEquippedArmor);
                CharacterPanel.Instance.MySlectedButton = this;
                icon.color = Color.grey;
            }
        }
    }

    public void EquipArmor(Armor armor)
    {
//        armor.Remove();

        if (MyEquippedArmor != null)
        {
            if (MyEquippedArmor != armor)
            {
//                armor.Slot.AddItem(MyEquippedArmor);
            }
       
//            UIManager.Instance.RefreshTooltip(MyEquippedArmor);
        }
        else
        {
            UIManager.Instance.HideTooltip();
        }

        icon.enabled = true;
        icon.sprite = armor.Icon;
        icon.color = Color.white;
        this.equippedArmor = armor; // 对装备的引用
//        this.MyEquippedArmor.CharButton = this;

        if (HandScript.Instance.Moveable == (armor as IMoveable))
        {
            HandScript.Instance.Drop();
        }

        if (gearSocket != null && MyEquippedArmor.AnimationClips != null)
        {
            gearSocket.Equip(MyEquippedArmor.AnimationClips);
        }
    
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MyEquippedArmor != null)
        {
//            UIManager.Instance.ShowTooltip(new Vector2(0, 0),transform.position, MyEquippedArmor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }

    public void DequipArmor()
    {
        icon.color = Color.white;
        icon.enabled = false;
       
        if (gearSocket != null && MyEquippedArmor.AnimationClips != null)
        {
            gearSocket.Dequip();
        }

//        equippedArmor.CharButton = null;
        equippedArmor = null;
    }
}
