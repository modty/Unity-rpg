
using UnityEngine;
using UnityEngine.UI;

public class LOLBagBarButtonScript:MonoBehaviour
{
    [SerializeField]
    private Image icon;
    public Image Icon => icon;

    public void openCloseInventory()
    {
        LOLInventoryScript.Instance.OpenClose();
    }
}
