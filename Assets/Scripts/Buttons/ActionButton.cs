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
    public Image MyIcon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    [SerializeField]
    private Image icon;

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

    /// <summary>
    /// 鼠标移动到按钮上
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUseable)
            {
                SetUseable(HandScript.MyInstance.MyMoveable as IUseable); 
            }
        }
    }
    /// <summary>
    /// 设置该按钮可用
    /// </summary>
    public void SetUseable(IUseable useable)
    {
        this.MyUseable = useable;

        UpdateVisual();
    }
    /// <summary>
    /// 更新按钮的图标等数据
    /// </summary>
    public void UpdateVisual() 
    {
        MyIcon.sprite = HandScript.MyInstance.Put().MyIcon;
        MyIcon.color = Color.white;
    }
}