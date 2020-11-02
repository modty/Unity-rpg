using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LOLInventoryButtonScript:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]private Image icon;
    public Image Icon => icon;
    private ItemInGame itemInGame;

    public ItemInGame ItemInGame
    {
        get => itemInGame;
        set => itemInGame = value;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemInGame != null)
        {
            LOLPointMesScript.Instance.ShowMes(string.Format("<color="+DataManager.Instance.GetQuality(ItemInGame.Uid).color+">"+ItemInGame.Name+"</color>"));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemInGame!=null)
        {
            LOLPointMesScript.Instance.Close();
        }
    }

}
