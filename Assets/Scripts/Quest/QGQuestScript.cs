using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 13

public class QGQuestScript : MonoBehaviour {

	public Quest MyQuest { get; set; }

    public void Select()
    {
        QuestGiverWindow.MyInstance.ShowQuestInfo(MyQuest);
    }
}
