using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    private TMP_Text _waveCountText;
    private TMP_Text _timeText;
    private Slider _playerHealth;
    private Slider _heatBar;
    private TMP_Text _heatText;
    private TMP_Text _playerNotifyText;
    private GameManager _gameManager;


    // Set up references to the UI elements ---------------------------------------------------------------------------------------------------
    void Awake()
    {
        _waveCountText = transform.Find("WaveCount").gameObject.GetComponent<TMP_Text>();
        _timeText = transform.Find("TimeDisplay").gameObject.GetComponent<TMP_Text>();
        _playerHealth = transform.Find("HealthBar").gameObject.GetComponent<Slider>();
        _heatBar = transform.Find("HeatBar").gameObject.GetComponent<Slider>();
        _heatText = transform.Find("HeatBarText").gameObject.GetComponent<TMP_Text>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _playerNotifyText = transform.Find("PlayerNotify").gameObject.GetComponent<TMP_Text>();
    }

    public void ResetDisplay()
    {
        _timeText.text = "";
        _playerHealth.value = 100;
        _heatBar.value = 50;
        _waveCountText.text = "";
        _playerNotifyText.text = "Press [return] to start the game!";
    }

    void Start()
    {
        ResetDisplay();
    }

    // public update methods for UI elements ---------------------------------------------------------------------------------------------------
    public void UpdateTimeDisplay(float counter)
    {
        if (counter == -1)
        { //clear the time display if passed arg is -1
            _timeText.text = "";
            return;
        }
        _timeText.text = counter.ToString("F1");
    }

    public void UpdateHealthDisplay(float currentHealth)
    {
        _playerHealth.value = currentHealth;
    }

    public void UpdateWaveDisplay(int waveNumber)
    {
        if (waveNumber == -1)
        {
            _waveCountText.text = "Next wave in:";
            return;
        }
        _waveCountText.text = "Wave: " + waveNumber;
    }

    public void UpdateHeatDisplay(float currentHeat)
    {
        _heatBar.value = currentHeat;
    }

    public void UpdateTemperatureText(float currentHeat)
    {
        if (currentHeat >= _heatBar.maxValue / 2)
        {
            _heatText.text = "HOT";
            _heatText.color = Color.red;

        }
        else
        {
            _heatText.text = "COLD";
            _heatText.color = Color.blue;
        }
    }

    public void UpdatePlayerNotifyText(string message)
    {
        _playerNotifyText.text = message;
    }

}
