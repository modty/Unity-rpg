﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerData PlayerData { get; set; }

    public List<ChestData> ChestData { get; set; }

    public List<EquipmentData> EquipmentData { get; set; }

    public InventoryData InventoryData { get; set; }

    public List<ActionButtonData> ActionButtonData { get; set; }

    public List<QuestData> QuestData { get; set; }

    public List<QuestGiverData> QuestGiverData { get; set; }

    public DateTime DateTime { get; set; }

    public string Scene { get; set; }

    public SaveData()
    {
        InventoryData = new InventoryData();
        ChestData = new List<ChestData>();
        ActionButtonData = new List<ActionButtonData>();
        EquipmentData = new List<EquipmentData>();
        QuestData = new List<QuestData>();
        QuestGiverData = new List<QuestGiverData>();
        DateTime = DateTime.Now;
    }
}

[Serializable]
public class PlayerData
{
    public int Level { get; set; }

    public float Xp { get; set; }

    public float MaxXP { get; set; }

    public float Health { get; set; }

    public float MaxHealth { get; set; }

    public float Mana { get; set; }

    public float MaxMana { get; set; }

    public float X { get; set; }

    public float Y { get; set; }

    public PlayerData(int level, float xp, float maxXp, float health, float maxHealth, float mana, float maxMana, Vector2 position)
    {
        this.Level = level;
        this.Xp = xp;
        this.MaxXP = maxXp;
        this.Health = health;
        this.MaxHealth = maxHealth;
        this.Mana = mana;
        this.MaxMana = maxMana;
        this.X = position.x;
        this.Y = position.y;

    }
}

[Serializable]
public class ItemData
{
    public string Titel { get; set; }

    public int StackCount { get; set; }

    public int SlotIndex { get; set; }

    public int BagIndex { get; set; }

    public ItemData(string titel, int stackCount = 0, int slotIndex = 0, int bagIndex = 0)
    {
        BagIndex = bagIndex;
        Titel = titel;
        StackCount = stackCount;
        SlotIndex = slotIndex;
    }
}

[Serializable]
public class ChestData
{
    public string Name { get; set; }

    public List<ItemData> Items { get; set; }

    public ChestData(string name)
    {
        Name = name;

        Items = new List<ItemData>();
    }
}

[Serializable]
public class InventoryData
{
    public List<BagData> Bags { get; set; }

    public List<ItemData> Items { get; set; }

    public InventoryData()
    {
        Bags = new List<BagData>();
        Items = new List<ItemData>();
    }
}

[Serializable]
public class BagData
{
    public int SlotCount { get; set; }
    public int BagIndex { get; set; }

    public BagData(int count, int index)
    {
        SlotCount = count;
        BagIndex = index;

    }
}

[Serializable]
public class EquipmentData
{
    public string Title { get; set; }

    public string Type { get; set; }

    public EquipmentData(string title, string type)
    {
        Title = title;
        Type = type;
    }

}

[Serializable]
public class ActionButtonData
{
    public string Action { get; set; }

    public bool IsItem { get; set; }

    public int Index { get; set; }

    public ActionButtonData(string action, bool isItem, int index)
    {
        this.Action = action;
        this.IsItem = isItem;
        this.Index = index;
    }
}

[Serializable]
public class QuestData
{
    public string Title { get; set; }

    public string Description { get; set; }

    public CollectObjective[] CollectObjectives { get; set; }

    public KillObjective[] KillObjectives { get; set; }

    public int QuestGiverID { get; set; }

    public QuestData(string title, string description, CollectObjective[] collectObjectives, KillObjective[] killObjectives, int questGiverID)
    {
        Title = title;

        Description = description;

        CollectObjectives = collectObjectives;

        KillObjectives = killObjectives;

        QuestGiverID = questGiverID;
    }
}

[Serializable]
public class QuestGiverData
{
    public List<string> CompletedQuests { get; set; }

    public int QuestGiverID { get; set; }

    public QuestGiverData(int questGiverID, List<string> completedQuests)
    {
        this.QuestGiverID = questGiverID;
        CompletedQuests = completedQuests;
    }
}
