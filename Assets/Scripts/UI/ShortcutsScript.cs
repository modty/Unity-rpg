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
    
    private Dictionary<int,ShortCutButtonScript> shortCutButtonScripts;

    private int[] keyCodeBinds;

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

    public void Initial(Dictionary<int, ItemInGame> shortCutItems)
    {
        shortCutButtonScripts=new Dictionary<int, ShortCutButtonScript>();
        for (int i = 0; i < keyCodeBinds.Length; i++)
        {
            GameObject obj = Instantiate(shortCutPrefab, shortCutParent.transform);
            shortCutButtonScripts[keyCodeBinds[i]] = obj.GetComponent<ShortCutButtonScript>();
            if (shortCutItems[keyCodeBinds[i]] != null)
            {
                shortCutButtonScripts[keyCodeBinds[i]].Icon.enabled = true;
                shortCutButtonScripts[keyCodeBinds[i]].Icon.sprite = shortCutItems[keyCodeBinds[i]].Icon;
                shortCutButtonScripts[keyCodeBinds[i]].ItemInGame = shortCutItems[keyCodeBinds[i]];
            }
            shortCutButtonScripts[keyCodeBinds[i]].BindKey.enabled = true;
            shortCutButtonScripts[keyCodeBinds[i]].BindKey.text = keyCodeBinds[i].ToString();
        }
            
    }

    public void OutUse(int keyCode)
    {
        shortCutButtonScripts[keyCode].ItemUse();
    }
    
}
