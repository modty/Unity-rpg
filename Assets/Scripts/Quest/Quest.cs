using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{

    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives;

    [SerializeField]
    private KillObjective[] killObjectives;

    [SerializeField]
    private int level;

    [SerializeField]
    private int xp;

    public QuestScript QuestScript { get; set; }

    public QuestGiver QuestGiver { get; set; }


    public string Title
    {
        get
        {
            return title;
        }

        set
        {
            title = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public CollectObjective[] CollectObjectives
    {
        get
        {
            return collectObjectives;
        }
    }

    public bool IsComplete
    {
        get
        {

            foreach (Objective o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }

            foreach (Objective o in KillObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }


            return true;
        }
    }

    public KillObjective[] KillObjectives
    {
        get
        {
            return killObjectives;
        }
        set
        { killObjectives = value; }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public int Xp
    {
        get
        {
            return xp;
        }

    }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;

    private int currentAmount;

    [SerializeField]
    private string type;

    public int Amount
    {
        get
        {
            return amount;
        }
    }

    public int CurrentAmount
    {
        get
        {
            return currentAmount;
        }

        set
        {
            currentAmount = value;
        }
    }

    public string Type
    {
        get
        {
            return type;
        }
    }

    public bool IsComplete
    {
        get
        {
            return CurrentAmount >= Amount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (Type.ToLower() == item.Title.ToLower())
        {
            CurrentAmount = InventoryScript.Instance.GetItemCount(item.Title);

            if (CurrentAmount <= Amount)
            {
                MessageFeedManager.Instance.WriteMessage(string.Format("{0}: {1}/{2}", item.Title, CurrentAmount, Amount));
            }



            Questlog.Instance.CheckCompletion();
            Questlog.Instance.UpdateSelected();
        }
    }

    public void UpdateItemCount()
    {
        CurrentAmount = InventoryScript.Instance.GetItemCount(Type);

        Questlog.Instance.CheckCompletion();
        Questlog.Instance.UpdateSelected();
    }

    public void Complete()
    {
        Stack<Item> items = InventoryScript.Instance.GetItems(Type, Amount);

        foreach (Item item in items)
        {
            item.Remove();
        }
    }


}

[System.Serializable]
public class KillObjective : Objective
{

    public void UpdateKillCount(Character character)
    {
        if (Type == character.Type)
        {
            if (CurrentAmount < Amount)
            {
                CurrentAmount++;
                MessageFeedManager.Instance.WriteMessage(string.Format("{0}: {1}/{2}", character.Type, CurrentAmount, Amount));
                Questlog.Instance.CheckCompletion();
                Questlog.Instance.UpdateSelected();
            }
        }
    }

}
