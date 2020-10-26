﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questlog : MonoBehaviour
{

    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questParent;

    private Quest selected;

    [SerializeField]
    private Text questDescription;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Text questCountTxt;

    [SerializeField]
    private int maxCount;

    private int currentCount;

    private List<QuestScript> questScripts = new List<QuestScript>();

    private List<Quest> quests = new List<Quest>();

    private static Questlog instance;

    public static Questlog Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Questlog>();
            }
            return instance;
        }

    }

    public List<Quest> Quests
    {
        get
        {
            return quests;
        }

        set
        {
            quests = value;
        }
    }

    private void Start()
    {
        questCountTxt.text = currentCount + "/" + maxCount;
    }

    public void AcceptQuest(Quest quest)
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            questCountTxt.text = currentCount + "/" + maxCount;
            foreach (CollectObjective o in quest.CollectObjectives)
            {
                InventoryScript.Instance.itemCountChangedEvent += new ItemCountChanged(o.UpdateItemCount);

                o.UpdateItemCount();
            }
            foreach (KillObjective o in quest.KillObjectives)
            {
                GameManager.Instance.killConfirmedEvent += new KillConfirmed(o.UpdateKillCount);
            }

            Quests.Add(quest);

            GameObject go = Instantiate(questPrefab, questParent);

            QuestScript qs = go.GetComponent<QuestScript>();
            quest.QuestScript = qs;
            qs.Quest = quest;

            questScripts.Add(qs);

            go.GetComponent<Text>().text = quest.Title;

            CheckCompletion();
        }



    }

    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.QuestScript.DeSelect();
            }

            string objectives = string.Empty;

            selected = quest;

            string title = quest.Title;

            foreach (Objective obj in quest.CollectObjectives)
            {
                objectives += obj.Type + ": " + obj.CurrentAmount + "/" + obj.Amount + "\n";
            }
            foreach (Objective obj in quest.KillObjectives)
            {
                objectives += obj.Type + ": " + obj.CurrentAmount + "/" + obj.Amount + "\n";
            }

            questDescription.text = string.Format("{0}\n<size=10>{1}</size>\n\nObjectives\n<size=10>{2}</size>", title, quest.Description, objectives);
        }


    }

    public void CheckCompletion()
    {
        foreach (QuestScript qs in questScripts)
        {
            qs.Quest.QuestGiver.UpdateQuestStatus();
            qs.IsComplete();
        }
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha == 1)
        {
            Close();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void AbandonQuest()
    {

        foreach (CollectObjective o in selected.CollectObjectives)
        {
            InventoryScript.Instance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
        }

        foreach (KillObjective o in selected.KillObjectives)
        {
            GameManager.Instance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);

        }

        RemoveQuest(selected.QuestScript);
    }

    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        Quests.Remove(qs.Quest);
        questDescription.text = string.Empty;
        selected = null;
        currentCount--;
        questCountTxt.text = currentCount + "/" + maxCount;
        qs.Quest.QuestGiver.UpdateQuestStatus();
        qs = null;
    }

    public bool HasQuest(Quest quest)
    {
        return Quests.Exists(x => x.Title == quest.Title);
    }
}
