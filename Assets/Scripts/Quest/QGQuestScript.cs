using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QGQuestScript : MonoBehaviour {

	public Quest Quest { get; set; }

    public void Select()
    {
        QuestGiverWindow.Instance.ShowQuestInfo(Quest);
    }
}
