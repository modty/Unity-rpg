using System.Collections.Generic;
using UnityEngine;


//39

public class LootTable : MonoBehaviour
{
    [SerializeField]
    private Loot[] loot;

    private List<Item> droppedItems = new List<Item>();

    private bool rolled = false;

    public void ShowLoot()
    {
        if (!rolled)
        {
            RollLoot();
        }


        LootWindow.MyInstance.CreatePages(droppedItems);
    }

    // 根据概率添加战利品
    private void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);

            if (roll <= item.MyDropChance)
            {
                droppedItems.Add(item.MyItem);
            }
        }

        rolled = true;
    }
}