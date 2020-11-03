
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LOLBagBarButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IDragHandler
{
    [SerializeField] private Image icon;
    public Image Icon => icon;
    private ItemInGame _itemInGame;

    public ItemInGame ItemInGame
    {
        get => _itemInGame;
        set => _itemInGame = value;
    }

    public void openCloseInventory()
    {
        LOLInventoryScript.Instance.OpenClose(ItemInGame);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemInGame != null)
        {
            LOLMesPlaneScript.Instance.ShowItemMes(string.Format("<color="+DataManager.Instance.GetQuality(ItemInGame.Uid).color+">"+ItemInGame.Name+"</color>"));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemInGame!=null)
        {
            LOLMesPlaneScript.Instance.CloseItemMes();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ItemInGame!=null)
        {
            LOLMesPlaneScript.Instance.PointIconShow(ItemInGame,GetComponent<RectTransform>().sizeDelta);
        }
    }

}

