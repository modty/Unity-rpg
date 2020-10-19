using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QGQuestScript : MonoBehaviour {

	public Quest MyQuest { get; set; }

    public void Select()
    {
        QuestGiverWindow.MyInstance.ShowQuestInfo(MyQuest);
    }
}
