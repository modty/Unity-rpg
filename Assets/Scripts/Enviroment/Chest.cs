using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite openSprite, closedSprite;

    private bool isOpen;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private List<Item> items;

    [SerializeField]
    private BagScript bag;

    public List<Item> MyItems
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    public BagScript MyBag
    {
        get
        {
            return bag;
        }

        set
        {
            bag = value;
        }
    }

    private void Awake()
    {
        items = new List<Item>();
    }

    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            AddItems();
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

    }

    public void StopInteract()
    {
        if (isOpen)
        {
            StoreItems();
            MyBag.Clear();
            isOpen = false;
            spriteRenderer.sprite = closedSprite;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }


    }

    public void AddItems()
    {
        if (MyItems != null)
        {
            foreach (Item item in MyItems)
            {
                item.Slot.AddItem(item);
            }
        }
    }

    public void StoreItems()
    {
        MyItems = MyBag.GetItems();
    }

}
