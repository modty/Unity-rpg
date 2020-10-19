using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag",menuName ="Items/Bag",order =1)]
public class Bag : Item, IUseable
{
    /// <summary>
    /// 背包有的格子数
    /// </summary>
    [SerializeField]
    private int slots;

    /// <summary>
    ///背包预制体
    /// </summary>
    [SerializeField]
    private GameObject bagPrefab;

    public BagScript MyBagScript { get; set; }

    /// <summary>
    /// 背包绑定的按钮
    /// </summary>
    public BagButton MyBagButton { get; set; }

    /// <summary>
    /// 背包所有的格子数
    /// </summary>
    public int MySlotCount
    {
        get
        {
            return slots;
        }
    }

    /// <summary>
    /// 初始化背包
    /// </summary>
    /// <param name="slots"></param>
    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    /// <summary>
    /// 装备背包（使用背包）
    /// </summary>
    public void Use()
    {
        if (InventoryScript.MyInstance.CanAddBag)
        {
            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);

            if (MyBagButton == null)
            {
                InventoryScript.MyInstance.AddBag(this);
            }
            else
            {
                InventoryScript.MyInstance.AddBag(this,MyBagButton);
            }

            MyBagScript.MyBagIndex = MyBagButton.MyBagIndex;
        }
 
    }

    public void SetupScript()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlots(slots);
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n{0} slot bag", slots);
    }
}
