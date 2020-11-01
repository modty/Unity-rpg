using System;
using Items;
using UnityEngine;

public class LOLInventoryScript:MonoBehaviour
{
    private static LOLInventoryScript instance;
    private GameObject obj;
    public static LOLInventoryScript Instance => instance;
    private ItemInGame itemInGame;
    private LOLInventoryButtonScript[] inventoryButtons;
    [SerializeField] private GameObject slots;
    [SerializeField] private GameObject slotPrefab;
    private void Awake()
    {
        instance = this;
    }


    public void LoadInventory(ItemInGame itemInGame)
    {
        this.itemInGame = itemInGame;
        ItemInGame[] itemInGames = itemInGame.ContainItems;
        inventoryButtons=new LOLInventoryButtonScript[itemInGames.Length];
        for (int i = 0; i < itemInGames.Length; i++)
        {
            GameObject obj=Instantiate(slotPrefab, slots.transform);
            inventoryButtons[i] = obj.GetComponent<LOLInventoryButtonScript>();
            if (itemInGames[i] != null&&inventoryButtons[i]!=null)
            {
                inventoryButtons[i].Icon.sprite = itemInGames[i].Icon;
            }
        }
    }
    
    public void OpenClose()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    
}
