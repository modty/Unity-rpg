using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 技能名字，将会根据该名字获取技能信息
    /// </summary>
    [SerializeField]
    private string spellName;

    public void OnPointerClick(PointerEventData eventData)
    {
        // 鼠标左键点击
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //将被点击物体放到鼠标上
            HandScript.MyInstance.TakeMoveable(SpellBook.MyInstance.GetSpell(spellName));
        }
    }
}