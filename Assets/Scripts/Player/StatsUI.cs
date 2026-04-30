using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class StatsUI : MonoBehaviour
{
    public static StatsUI Instance { get; private set; }

    public GameObject[] statsSlot;
    public CanvasGroup statsCanvas;
    private bool statsOpen = false;

    private void Start()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        UpdateAllStats();
    }

    private void Update()   
    {
        if(Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (statsOpen) CloseStats();
            else OpenStats();
        }
    }

    public void CloseStats()
    {
        //UpdateAllStats();
        statsCanvas.alpha = 0;
        statsCanvas.interactable = false;
        statsCanvas.blocksRaycasts = false;
        statsOpen = false;
        Time.timeScale = 1;
    }

    public void OpenStats()
    {
        UpdateAllStats();
        Time.timeScale = 0;
        statsCanvas.alpha = 1;
        statsCanvas.interactable = true;
        statsCanvas.blocksRaycasts = true;
        statsOpen = true;
    }

    public void UpdateDamage()
    {
        statsSlot[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.damage;
    }

    public void UpdateSpeed()
    {
        statsSlot[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.speed;
    }

    public void UpdateCurrentHealth()
    {
        Debug.Log("Updating health stat in UI: " + StatsManager.Instance.currentHealth);
        statsSlot[2].GetComponentInChildren<TMP_Text>().text = "Health: " + StatsManager.Instance.currentHealth;
    }   

    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
        UpdateCurrentHealth();
    }
}
