using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SCTTYPE {DAMAGE,HEAL}

public class CombatTextManager : MonoBehaviour
{

    private static CombatTextManager instance;

    public static CombatTextManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CombatTextManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject combatTextPrefab;

    public void CreateText(Vector2 position, string text, SCTTYPE type, bool crit)
    {
        // 偏移量
        position.y += 0.8f;
        Text sct = Instantiate(combatTextPrefab, transform).GetComponent<Text>();
        sct.transform.position = position;

        string operation = string.Empty;

        switch (type)
        {
            case SCTTYPE.DAMAGE:
                operation += "-";
                sct.color = Color.red;
                break;
            case SCTTYPE.HEAL:
                operation += "+";
                sct.color = Color.green;
                break;
        }

        sct.text = operation + text;

        if (crit)
        {
            sct.GetComponent<Animator>().SetBool("Crit", crit);
        }
    }


}
