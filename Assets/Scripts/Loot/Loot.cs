using UnityEngine;

// 29

[System.Serializable]
public class Loot
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private float dropChance;

    public Item MyItem
    {
        get
        {
            return item;
        }
    }

    public float MyDropChance
    {
        get
        {
            return dropChance;
        }
    }
}