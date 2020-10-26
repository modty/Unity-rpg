﻿﻿using System.Collections;
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

    private int Amount
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
        InventoryScript.Instance.itemCountChangedEvent += new ItemCountChanged(UpdateMaterialCount);
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

        title.text = recipe.Output.Title;

        description.text = recipe.Description + " " + recipe.Output.Title.ToLower();

        craftItemInfo.Initialize(recipe.Output, 1);

        foreach (CraftingMaterial material in recipe.Materials)
        {
            GameObject tmp = Instantiate(materialPrefab, parent);

            tmp.GetComponent<ItemInfo>().Initialize(material.Item, material.Count);

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
              
                Amount = 1;
                
            }
            else if (int.Parse(countTxt.text) > maxAmount)
            {
                Amount = maxAmount;
            }
        }
        else
        {
            Amount = 0;
            maxAmount = 0;
        }
    }

    public void Craft(bool all)
    {
      
        if (CanCraft() && !Player.Instance.IsAttacking)
        {
            if (all)
            {
                amounts.Sort();
                countTxt.text = maxAmount.ToString();
                StartCoroutine(CraftRoutine(amounts[0]));
            }
            else
            {
                StartCoroutine(CraftRoutine(Amount));
            }

       
        }

        
    }

    private bool CanCraft()
    {
        bool canCraft = true;

        amounts = new List<int>();

        foreach (CraftingMaterial material in selectedRecipe.Materials)
        {
            int count = InventoryScript.Instance.GetItemCount(material.Item.Title);

            if (count >= material.Count)
            {
                amounts.Add(count/material.Count);
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
            Amount += i;
        }
    }

    private IEnumerator CraftRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Player.Instance.MyInitRoutine = StartCoroutine(Player.Instance.CraftRoutine(selectedRecipe));
        }
    }

    public void AdddItemsToInventory()
    {
        if (InventoryScript.Instance.AddItem(craftItemInfo.Item))
        {
            foreach (CraftingMaterial material in selectedRecipe.Materials)
            {
                for (int i = 0; i < material.Count; i++)
                {
                    InventoryScript.Instance.RemoveItem(material.Item);
                }
            }
        }

      
    }
}
