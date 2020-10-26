using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
    [SerializeField]
    private Text dateTime;

    [SerializeField]
    private Image health;

    [SerializeField]
    private Image mana;

    [SerializeField]
    private Image xp;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text manaText;

    [SerializeField]
    private Text xpText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private GameObject visuals;

    [SerializeField]
    private int index;

    public int Index
    {
        get
        {
            return index;
        }
    }

    private void Awake()
    {
        visuals.SetActive(false);
    }

    public void ShowInfo(SaveData saveData)
    {

        visuals.SetActive(true);

        dateTime.text = "Date: " + saveData.DateTime.ToString("dd/MM/yyy") + " - Time: " + saveData.DateTime.ToString("H:mm");
        health.fillAmount = saveData.PlayerData.Health / saveData.PlayerData.MaxHealth;
        healthText.text = saveData.PlayerData.Health + " / " +saveData.PlayerData.MaxHealth;

        mana.fillAmount = saveData.PlayerData.Mana / saveData.PlayerData.MaxMana;
        manaText.text = saveData.PlayerData.Mana + " / " + saveData.PlayerData.MaxMana;

        xp.fillAmount = saveData.PlayerData.Xp / saveData.PlayerData.MaxXP;
        xpText.text = saveData.PlayerData.Xp + " / " + saveData.PlayerData.MaxXP;

        levelText.text = saveData.PlayerData.Level.ToString();
    }

    public void HideVisuals()
    {
        visuals.SetActive(false);
    }

}
