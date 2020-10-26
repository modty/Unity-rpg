﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private Item[] items;

    private Chest[] chests;

    private CharButton[] equipment;

    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private SavedGame[] saveSlots;

    [SerializeField]
    private GameObject dialogue;

    [SerializeField]
    private Text dialogueText;

    private SavedGame current;

    private string action;

    void Awake()
    {
        chests = FindObjectsOfType<Chest>();
        equipment = FindObjectsOfType<CharButton>();

        foreach (SavedGame saved in saveSlots)
        {
            ShowSavedFiles(saved);
        }

    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Load"))
        {
            Load(saveSlots[PlayerPrefs.GetInt("Load")]);
            PlayerPrefs.DeleteKey("Load");
        }
        else
        {
            Player.Instance.SetDefaultValues();
        }
    }

    public void ShowDialogue(GameObject clickButton)
    {
        action = clickButton.name;

        switch (action)
        {
            case "Load":
                dialogueText.text = "加载存档?";
                break;
            case "Save":
                dialogueText.text = "保存游戏?";
                break;
            case "Delete":
                dialogueText.text = "删除存档?";
                break;
        }

        current = clickButton.GetComponentInParent<SavedGame>();
        dialogue.SetActive(true);
    }

    public void ExecuteAction()
    {
        switch (action)
        {
            case "Load":
                LoadScene(current);
                break;
            case "Save":
                Save(current);
                break;
            case "Delete":
                Delete(current);
                break;
        }

        CloseDialogue();

    }

    private void LoadScene(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            PlayerPrefs.SetInt("Load", savedGame.Index);
            SceneManager.LoadScene(data.Scene);
        }
    }

    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }

    private void Delete(SavedGame savedGame)
    {
        File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");
        savedGame.HideVisuals();
    }

    private void ShowSavedFiles(SavedGame savedGame)
    {
   
        if (File.Exists(Application.persistentDataPath + "/"+savedGame.gameObject.name+".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            savedGame.ShowInfo(data);
        }
    }

   public void Save(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name+".dat", FileMode.Create);

            SaveData data = new SaveData();

            data.Scene = SceneManager.GetActiveScene().name;

            SaveEquipment(data);

            SaveBags(data);

            SaveInventory(data);

            SavePlayer(data);

            SaveChests(data);

            SaveActionButtons(data);

            SaveQuests(data);

            SaveQuestGivers(data);

            bf.Serialize(file, data);

            file.Close();

            ShowSavedFiles(savedGame);


        }
        catch (System.Exception)
        {
            Delete(savedGame);
            PlayerPrefs.DeleteKey("Load");
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.PlayerData = new PlayerData(Player.Instance.Level,
            Player.Instance.Xp.CurrentValue, Player.Instance.Xp.MaxValue,
            Player.Instance.Health.CurrentValue, Player.Instance.Health.MaxValue,
            Player.Instance.Mana.CurrentValue, Player.Instance.Mana.MaxValue,
            Player.Instance.transform.position);
    }

    private void SaveChests(SaveData data)
    {
        for (int i = 0; i < chests.Length; i++)
        {
            data.ChestData.Add(new ChestData(chests[i].name));

            foreach (Item item in chests[i].MyItems)
            {
                if (chests[i].MyItems.Count > 0)
                {
                    data.ChestData[i].Items.Add(new ItemData(item.Title, item.Slot.Items.Count, item.Slot.Index));
                }
            }
        }
    }

    private void SaveBags(SaveData data)
    {
        for (int i = 1; i < InventoryScript.Instance.Bags.Count; i++)
        {
            data.InventoryData.Bags.Add(new BagData(InventoryScript.Instance.Bags[i].SlotCount, InventoryScript.Instance.Bags[i].BagButton.BagIndex));

        }
    }

    private void SaveEquipment(SaveData data)
    {

        foreach (CharButton charButton in equipment)
        {
            if (charButton.MyEquippedArmor != null)
            {
                data.EquipmentData.Add(new EquipmentData(charButton.MyEquippedArmor.Title, charButton.name));
            }
        }
    }

    private void SaveActionButtons(SaveData data)
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i].Useable != null)
            {
                ActionButtonData action;

                if (actionButtons[i].Useable is Spells)
                {
                    action = new ActionButtonData((actionButtons[i].Useable as Spells).Title, false, i);
                }
                else
                {
                    action = new ActionButtonData((actionButtons[i].Useable as Item).Title, true, i);
                }

                data.ActionButtonData.Add(action);
            }
        }
    }

    private void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.Instance.GetAllItems();

        foreach (SlotScript slot in slots)
        {
            data.InventoryData.Items.Add(new ItemData(slot.Item.Title, slot.Items.Count, slot.Index, slot.Bag.BagIndex));
        }

    }

    private void SaveQuests(SaveData data)
    {
        foreach (Quest quest in Questlog.Instance.Quests)
        {
            data.QuestData.Add(new QuestData(quest.Title, quest.Description, quest.CollectObjectives, quest.KillObjectives,quest.QuestGiver.QuestGiverID));
        }
    }

    private void SaveQuestGivers(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiver questGiver in questGivers)
        {
            data.QuestGiverData.Add(new QuestGiverData(questGiver.QuestGiverID, questGiver.CompltedQuests));
        }

    }


    private void Load(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();

            LoadEquipment(data);

            LoadBags(data);

            LoadInventory(data);

            LoadPlayer(data);

            LoadChests(data);

            LoadActionButtons(data);

            LoadQuests(data);

            LoadQuestGiver(data);

        }
        catch (System.Exception)
        {
            Delete(savedGame);
            PlayerPrefs.DeleteKey("Load");
            SceneManager.LoadScene(0);
        }
    }

    private void LoadPlayer(SaveData data)
    {
        Player.Instance.Level = data.PlayerData.Level;
        Player.Instance.UpdateLevel();
        Player.Instance.Health.Initialize(data.PlayerData.Health, data.PlayerData.MaxHealth);
        Player.Instance.Mana.Initialize(data.PlayerData.Mana, data.PlayerData.MaxMana);
        Player.Instance.Xp.Initialize(data.PlayerData.Xp, data.PlayerData.MaxXP);
        Player.Instance.transform.position = new Vector2(data.PlayerData.X, data.PlayerData.Y);

    }

    private void LoadChests(SaveData data)
    {
        foreach (ChestData chest in data.ChestData)
        {
            Chest c = Array.Find(chests, x => x.name == chest.Name);

            foreach (ItemData itemData in chest.Items)
            {
                Item item = Instantiate(Array.Find(items, x => x.Title == itemData.Titel));
                item.Slot = c.MyBag.Slots.Find(x => x.Index == itemData.SlotIndex);
                c.MyItems.Add(item);
            }
        }

    }

    private void LoadBags(SaveData data)
    {
        foreach (BagData bagData in data.InventoryData.Bags)
        {
            Bag newBag = (Bag)Instantiate(items[0]);

            newBag.Initialize(bagData.SlotCount);

            InventoryScript.Instance.AddBag(newBag, bagData.BagIndex);
        }
    }

    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.EquipmentData)
        {
            CharButton cb = Array.Find(equipment, x => x.name == equipmentData.Type);

            cb.EquipArmor(Array.Find(items, x => x.Title == equipmentData.Title) as Armor);
        }
    }

    private void LoadActionButtons(SaveData data)
    {
        foreach (ActionButtonData buttonData in data.ActionButtonData)
        {
            if (buttonData.IsItem)
            {
                actionButtons[buttonData.Index].SetUseable(InventoryScript.Instance.GetUseable(buttonData.Action));
            }
            else
            {
                actionButtons[buttonData.Index].SetUseable(SpellBook.Instance.GetSpell(buttonData.Action));
            }
        }
    }

    private void LoadInventory(SaveData data)
    {
        foreach (ItemData itemData in data.InventoryData.Items)
        {
            Item item = Instantiate(Array.Find(items, x => x.Title == itemData.Titel));

            for (int i = 0; i < itemData.StackCount; i++)
            {
                InventoryScript.Instance.PlaceInSpecific(item, itemData.SlotIndex, itemData.BagIndex);
            }
        }
    }

    private void LoadQuests(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestData questData in data.QuestData)
        {
            QuestGiver qg = Array.Find(questGivers, x => x.QuestGiverID == questData.QuestGiverID);
            Quest q = Array.Find(qg.Quests, x => x.Title == questData.Title);
            q.QuestGiver = qg;
            q.KillObjectives = questData.KillObjectives;
            Questlog.Instance.AcceptQuest(q);
        }
    }

    private void LoadQuestGiver(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiverData questGiverData in data.QuestGiverData)
        {
            QuestGiver questGiver = Array.Find(questGivers, x => x.QuestGiverID == questGiverData.QuestGiverID);
            questGiver.CompltedQuests = questGiverData.CompletedQuests;
            questGiver.UpdateQuestStatus();
        }
    }
}
