﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour {

    public Quest Quest { get; set; }

    private bool markedComplete = false;

    public void Select()
    {
        GetComponent<Text>().color = Color.red;
        Questlog.Instance.ShowDescription(Quest);
    }

    public void DeSelect()
    {
        GetComponent<Text>().color = Color.white;
    }

    public void IsComplete()
    {
        if (Quest.IsComplete && !markedComplete)
        {
            markedComplete = true;
            GetComponent<Text>().text = "[" + Quest.Level + "] " + Quest.Title +"(C)";
            MessageFeedManager.Instance.WriteMessage(string.Format("{0} (完成)", Quest.Title));
        }
        else if (!Quest.IsComplete)
        {
            markedComplete = false;
            GetComponent<Text>().text = "[" + Quest.Level + "] " + Quest.Title;
        }



    }
}
