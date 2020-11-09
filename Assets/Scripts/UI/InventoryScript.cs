using System;
using Items;
using UnityEngine;

public class InventoryScript:MonoBehaviour
{
    private static InventoryScript instance;
    private GameObject obj;
    public static InventoryScript Instance => instance;
    private ItemInGame itemInGame;
    private InventoryButtonScript[] inventoryButtons;
    [SerializeField] private GameObject slots;
    [SerializeField] private GameObject slotPrefab;

    public ItemInGame ItemInGame
    {
        get => itemInGame;
        set => itemInGame = value;
    }

    private void Awake()
    {
        instance = this;
    }


    public void LoadInventory(ItemInGame itemInGame)
    {
        for (int i = 0; i < slots.transform.childCount; i++) {  
            Destroy (slots.transform.GetChild (i).gameObject);  
        }  
        this.itemInGame = itemInGame;
        ItemInGame[] itemInGames = itemInGame.ContainItems;
        inventoryButtons=new InventoryButtonScript[itemInGames.Length];
        for (int i = 0; i < itemInGames.Length; i++)
        {
            GameObject obj=Instantiate(slotPrefab, slots.transform);
            inventoryButtons[i] = obj.GetComponent<InventoryButtonScript>();
            if (itemInGames[i] != null&&inventoryButtons[i]!=null)
            {
                inventoryButtons[i].ItemInGame = itemInGames[i];
                inventoryButtons[i].Icon.sprite = itemInGames[i].Icon;
                inventoryButtons[i].Icon.enabled = true;
            }
        }
    }
    
    public void OpenClose(ItemInGame itemInGame)
    {
        if (ItemInGame.Equals(itemInGame))
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
        else if(itemInGame!=null)
        {
            LoadInventory(itemInGame);
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(!gameObject.activeSelf);
            }
        }
    }
    
}
