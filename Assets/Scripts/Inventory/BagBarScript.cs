
using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class BagBarScript:MonoBehaviour
{
    private static BagBarScript instance;

    [SerializeField]
    private GameObject prefab;

    private BagButton[] bags;

    public static BagBarScript Instance => instance;

    private void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// 背包装备栏初始化
    /// </summary>
    public void Initial(int size,Dictionary<int,ItemInGame> equipedBags)
    {
        bags = new BagButton[size];
        for (int i = 0; i < bags.Length; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            bags[i]=obj.GetComponent<BagButton>();
        }
        foreach (KeyValuePair<int,ItemInGame> pair in equipedBags)
        {
            BagButton bb = bags[pair.Key];
            bb.BagIndex = pair.Key;
            bb.ItemInGame = pair.Value;
            bags[pair.Key]=bb;
        }
    }
}
