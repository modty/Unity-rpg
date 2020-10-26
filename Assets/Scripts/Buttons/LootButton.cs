﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LootButton : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler
{

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    private LootWindow lootWindow;

    public Image Icon
    {
        get
        {
            return icon;
        }
    }

    public Text Title
    {
        get
        {
            return title;
        }

    }

    public Item Loot { get; set; }

    private void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 添加战利品
        if (InventoryScript.Instance.AddItem(Loot))
        {
            gameObject.SetActive(false);
            lootWindow.TakeLoot(Loot);
            UIManager.Instance.HideTooltip();
        }
     
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowTooltip(new Vector2(1,0), transform.position, Loot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
