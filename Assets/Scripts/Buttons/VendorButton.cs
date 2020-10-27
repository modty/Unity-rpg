using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text price;

    [SerializeField]
    private Text quantity;

    private VendorItem vendorItem;

    public void AddItem(VendorItem vendorItem)
    {
        this.vendorItem = vendorItem;

        if (vendorItem.Quantity > 0 ||(vendorItem.Quantity == 0 && vendorItem.Unlimited))
        {

            icon.sprite = vendorItem.ItemInGame.Icon;
            title.text = string.Format("<color={0}>{1}</color>", "#00ff00ff", vendorItem.ItemInGame.Name);

            if (!vendorItem.Unlimited)
            {
                quantity.text = vendorItem.Quantity.ToString();
            }
            else
            {
                quantity.text = string.Empty;
            }

            if (vendorItem.ItemInGame.Price > 0)
            {
                price.text = "价格: " + vendorItem.ItemInGame.Price.ToString();
            }
            else
            {
                price.text = string.Empty;
            }

            gameObject.SetActive(true);
        }

    }


    public void OnPointerClick(PointerEventData eventData)
    {
//        if ((Player.Instance.MyGold >= vendorItem.ItemInGame.Price) && InventoryScript.Instance.AddItem(Instantiate(vendorItem.ItemInGame)))
//        {
//            SellItem();
//        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
//        UIManager.Instance.ShowTooltip(new Vector2(0, 1), transform.position, vendorItem.ItemInGame);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }

    private void SellItem()
    {
        Player.Instance.MyGold -= vendorItem.ItemInGame.Price;

        if (!vendorItem.Unlimited)
        {
            vendorItem.Quantity--;
            quantity.text = vendorItem.Quantity.ToString();

            if (vendorItem.Quantity == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
