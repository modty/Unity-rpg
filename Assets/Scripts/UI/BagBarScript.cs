using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class BagBarScript : MonoBehaviour
{

    /// <summary>
    /// 背包显示UI板
    /// </summary>

    [SerializeField] private GameObject bagBarSlotPrefab;

    [SerializeField] private GameObject bagBarSlots;
    private static BagBarScript instance;

    public static BagBarScript Instance => instance;
    
    /// <summary>
    /// 背包装备栏
    /// </summary>
    private BagBarButtonScript[] bags;

    private ItemInGame[] bagDatas;
    
    private bool[] isEquiped;

    private bool isLoadInventory;

    public BagBarButtonScript[] Bags
    {
        get => bags;
        set => bags = value;
    }

    public ItemInGame[] BagDatas
    {
        get => bagDatas;
        set => bagDatas = value;
    }

    public bool[] IsEquiped
    {
        get => isEquiped;
        set => isEquiped = value;
    }

    /// <summary>
    /// 加载数据（模拟）
    /// </summary>
    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        LoadBagBar();
    }

    public void LoadBagBar()
    {
        for (int i = 0; i < bags.Length; i++)
        {
            GameObject obj = Instantiate(bagBarSlotPrefab, bagBarSlots.transform);
            bags[i]=obj.GetComponent<BagBarButtonScript>();
            if (isEquiped[i]&&bags[i]!=null)
            {
                bags[i].Icon.sprite = bagDatas[i].Icon;
                bags[i].Icon.enabled=true;
                bags[i].ItemInGame = bagDatas[i];
                if (!isLoadInventory)
                {
                    InventoryScript.Instance.LoadInventory(bagDatas[i]);
                    isLoadInventory = true;
                }
            }
        }
    }
    
    
}
