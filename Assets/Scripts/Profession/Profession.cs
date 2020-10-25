﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profession : MonoBehaviour
{
    [SerializeField]
    private Text title;

    [SerializeField]
    private Text description;

    [SerializeField]
    private GameObject materialPrefab;

    [SerializeField]
    private Transform parent;

    private List<GameObject> materials = new List<GameObject>();

    private List<int> amounts = new List<int>();

    [SerializeField]
    private Recipe selectedRecipe;

    [SerializeField]
    private Text countTxt;

    [SerializeField]
    private ItemInfo craftItemInfo;

    private int maxAmount;

    private int amount;

    private int MyAmount
    {
        set {
            countTxt.text = value.ToString();
            amount = value;
        }
        get {

            return amount;
        }

    }

    private void Start()
    {
        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateMaterialCount);
        ShowDescription(selectedRecipe);

    }

    public void ShowDescription(Recipe recipe)
    {
        if (selectedRecipe != null)
        {
            selectedRecipe.Deselect();
        }

        this.selectedRecipe = recipe;

        this.selectedRecipe.Select();

        foreach (GameObject gameObject in materials)
        {
            Destroy(gameObject);
        }

        materials.Clear();

        title.text = recipe.Output.MyTitle;

        description.text = recipe.MyDescription + " " + recipe.Output.MyTitle.ToLower();

        craftItemInfo.Initialize(recipe.Output, 1);

        foreach (CraftingMaterial material in recipe.Materials)
        {
            GameObject tmp = Instantiate(materialPrefab, parent);

            tmp.GetComponent<ItemInfo>().Initialize(material.MyItem, material.MyCount);

            materials.Add(tmp);

        }

        UpdateMaterialCount(null);
    }

    private void UpdateMaterialCount(Item item)
    {
        amounts.Sort();

        foreach (GameObject material in materials)
        {
            ItemInfo tmp = material.GetComponent<ItemInfo>();
            tmp.UpdateStackCount();
        }
        if (CanCraft())
        {
            maxAmount = amounts[0];

            if (countTxt.text == "0")
            {
              
                MyAmount = 1;
                
            }
            else if (int.Parse(countTxt.text) > maxAmount)
            {
                MyAmount = maxAmount;
            }
        }
        else
        {
            MyAmount = 0;
            maxAmount = 0;
        }
    }

    public void Craft(bool all)
    {
      
        if (CanCraft() && !Player.MyInstance.IsAttacking)
        {
            if (all)
            {
                amounts.Sort();
                countTxt.text = maxAmount.ToString();
                StartCoroutine(CraftRoutine(amounts[0]));
            }
            else
            {
                StartCoroutine(CraftRoutine(MyAmount));
            }

       
        }

        
    }

    private bool CanCraft()
    {
        bool canCraft = true;

        amounts = new List<int>();

        foreach (CraftingMaterial material in selectedRecipe.Materials)
        {
            int count = InventoryScript.MyInstance.GetItemCount(material.MyItem.MyTitle);

            if (count >= material.MyCount)
            {
                amounts.Add(count/material.MyCount);
                continue;
            }
            else
            {
                canCraft = false;
                break;
            }
            
        }

        return canCraft;
    }

    public void ChangeAmount(int i)
    {
        if ((amount + i) > 0 && amount + i <= maxAmount)
        {
            MyAmount += i;
        }
    }

    private IEnumerator CraftRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Player.MyInstance.MyInitRoutine = StartCoroutine(Player.MyInstance.CraftRoutine(selectedRecipe));
        }
    }

    public void AdddItemsToInventory()
    {
        if (InventoryScript.MyInstance.AddItem(craftItemInfo.MyItem))
        {
            foreach (CraftingMaterial material in selectedRecipe.Materials)
            {
                for (int i = 0; i < material.MyCount; i++)
                {
                    InventoryScript.MyInstance.RemoveItem(material.MyItem);
                }
            }
        }

      
    }
}
