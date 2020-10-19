using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 231

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
    
    public QuestScript MyQuestScript { get; set; }

    public QuestGiver MyQuestGiver { get; set; }

    public string MyTitle
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
    public int MyLevel
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

    public int MyXp
    {
        get
        {
            return xp;
        }

    }
    public string MyDescription
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

    public CollectObjective[] MyCollectObjectives
    {
        get
        {
            return collectObjectives;
        }
    }

    public bool IsComplete
    {
        get {

            foreach (Objective o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }

            foreach (Objective o in MyKillObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }


            return true;
        }
    }

    public KillObjective[] MyKillObjectives
    {
        get
        {
            return killObjectives;
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
    private string title;
    
    public int MyAmount
    {
        get
        {
            return amount;
        }
    }

    public int MyCurrentAmount
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

    public string MyType
    {
        get
        {
            return type;
        }
    }
    public string MyTitle
    {
        get
        {
            return title;
        }
        set { title = value; }
    }
    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (MyType.ToLower() == item.MyType)
        {
            MyTitle = item.MyTitle;
            MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(item.MyType);

            if (MyCurrentAmount <= MyAmount)
            {
                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", item.MyTitle, MyCurrentAmount, MyAmount));
            }

          

            Questlog.MyInstance.CheckCompletion();
            Questlog.MyInstance.UpdateSelected();
        }
    }

    public void UpdateItemCount()
    {
        MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(MyType);

        Questlog.MyInstance.CheckCompletion();
        Questlog.MyInstance.UpdateSelected();
    }

    public void Complete()
    {
        Stack<Item> items = InventoryScript.MyInstance.GetItems(MyType, MyAmount);

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
        if (MyType == character.MyType)
        {
            MyTitle = character.MyTitle;
            MyCurrentAmount++;
            MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", character.MyTitle, MyCurrentAmount, MyAmount));
            Questlog.MyInstance.CheckCompletion();
            Questlog.MyInstance.UpdateSelected();

        }
    }

}
