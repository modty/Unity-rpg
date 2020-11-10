using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class ShortcutsScript : MonoBehaviour
{
    private static ShortcutsScript instance;
    public static ShortcutsScript Instance => instance;
    [SerializeField]
    private GameObject shortCutPrefab;
    
    [SerializeField] 
    private GameObject shortCutParent;

    private Dictionary<int, ItemInGame> shortCutItems;
    private ShortCutButtonScript[] inventoryButtonScripts;
    private int[] keyCodeBinds;
    public Dictionary<int, ItemInGame> ShortCutItems
    {
        get => shortCutItems;
        set => shortCutItems = value;
    }

    private void Awake()
    {
        instance = this;
        keyCodeBinds=new int[6];
        keyCodeBinds[0] = 1;
        keyCodeBinds[1] = 2;
        keyCodeBinds[2] = 3;
        keyCodeBinds[3] = 5;
        keyCodeBinds[4] = 6;
        keyCodeBinds[5] = 7;
    }

    public void Initial()
    {
        inventoryButtonScripts=new ShortCutButtonScript[keyCodeBinds.Length];
        for (int i = 0; i < keyCodeBinds.Length; i++)
        {
            GameObject obj = Instantiate(shortCutPrefab, shortCutParent.transform);
            inventoryButtonScripts[i] = obj.GetComponent<ShortCutButtonScript>();
            if (shortCutItems[keyCodeBinds[i]] != null)
            {
                inventoryButtonScripts[i].Icon.enabled = true;
                inventoryButtonScripts[i].Icon.sprite = shortCutItems[keyCodeBinds[i]].Icon;
            }

            inventoryButtonScripts[i].BindKey.enabled = true;
            inventoryButtonScripts[i].BindKey.text = keyCodeBinds[i].ToString();
        }
            
    }
    
    
    
}
