using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 可供使用的按钮
    /// </summary>
    public IUseable MyUseable { get; set; }

    /// <summary>
    /// 引用当前对象的Button
    /// </summary>
    public Button MyButton { get; private set; }

    void Start ()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
    }
	
    void Update () {
		
    }


    public void OnClick()
    {
        if (MyUseable != null)
        {
            MyUseable.Use();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}