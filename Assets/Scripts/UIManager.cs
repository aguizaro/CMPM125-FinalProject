using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public enum UIState
{
    Main,
    Settings,
    Credits,
    HUD,
    Pause
}

public class UserSettings : MonoBehaviour
{
    public static string PlayerName { get; set; }
    public static float MouseSensitivity { get; set; }
    public static bool IsPaused { get; set; }

}

public class UIManager : MonoBehaviour
{

    // parent canvas
    [SerializeField] private Canvas _canvas;
    // UI States
    private readonly UIState _startState = UIState.Main;
    private UIState _previousState;
    private UIState _currentState;

    // Text fading
    [SerializeField] private float textFadeOutSeconds = 8f;
    private float fadeSec;

    // Mouse Sensitivity Slider
    private Slider _senseSlider;
    private TMP_Text _senseSliderVal;
    // nickname and projectlile size
    private TMP_InputField _nameInput;
    private TMP_Text _nameText;
    //Name placeholder text
    private TMP_Text _placeHolderText;

    private TMP_Text _killCount; // TMP Text not created yet for this

    // HUD elements
    private TMP_Text _waveCountText;
    private TMP_Text _timeText;
    private Slider _playerHealth;
    private Slider _heatBar;
    private TMP_Text _heatText;
    private TMP_Text _playerNotifyText;
    private GameManager _gameManager;
    private TMP_Text _killCountText;
    private TMP_Text _enemiesRemainingText;
    private TMP_Text _playerNameDisplay;


    // Set up references to the UI elements ---------------------------------------------------------------------------------------------------
    void Awake()
    {
        UserSettings.IsPaused = false;
        UserSettings.PlayerName = "Guest User";
        UserSettings.MouseSensitivity = 0.5f;

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _canvas.enabled = true;
        // Assign button controls
        _canvas.transform.Find("Main").transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(PlayGame);
        _canvas.transform.Find("Main").transform.Find("SettingsButton").GetComponent<Button>().onClick.AddListener(PlaySettings);
        _canvas.transform.Find("Main").transform.Find("CreditsButton").GetComponent<Button>().onClick.AddListener(PlayCredits);

        _canvas.transform.Find("Settings").transform.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener(ConfirmSettings);
        _canvas.transform.Find("Settings").transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(PlayPrev);

        _canvas.transform.Find("Credits").transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(PlayPrev);

        _canvas.transform.Find("Pause").transform.Find("ResumeButton").GetComponent<Button>().onClick.AddListener(PlayerUnPause);
        _canvas.transform.Find("Pause").transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(QuitGame);
        _canvas.transform.Find("Pause").transform.Find("SettingsButton").GetComponent<Button>().onClick.AddListener(PlaySettings);
        _canvas.transform.Find("Pause").transform.Find("CreditsButton").GetComponent<Button>().onClick.AddListener(PlayCredits);

        // Assign slider and input field controls
        _senseSlider = _canvas.transform.Find("Settings").transform.Find("SenseSlider").GetComponent<Slider>();
        _senseSliderVal = _canvas.transform.Find("Settings").transform.Find("SenseSlider").transform.Find("SenseVal").GetComponent<TMP_Text>();
        _nameInput = _canvas.transform.Find("Settings").transform.Find("NameInput").transform.Find("InputField").GetComponent<TMP_InputField>();
        _placeHolderText = _canvas.transform.Find("Settings").transform.Find("NameInput").transform.Find("InputField").transform.Find("TextArea").transform.Find("Placeholder").GetComponent<TMP_Text>();
        //_nameText = _canvas.transform.Find("HUD").transform.Find("PlayerName").GetComponent<TMP_Text>();
        _waveCountText = _canvas.transform.Find("HUD").transform.Find("WaveCount").GetComponent<TMP_Text>();
        _timeText = _canvas.transform.Find("HUD").transform.Find("TimeDisplay").GetComponent<TMP_Text>();
        _playerHealth = _canvas.transform.Find("HUD").transform.Find("HealthBar").GetComponent<Slider>();
        _heatBar = _canvas.transform.Find("HUD").transform.Find("HeatBar").GetComponent<Slider>();
        _heatText = _canvas.transform.Find("HUD").transform.Find("HeatBarText").GetComponent<TMP_Text>();
        _playerNotifyText = _canvas.transform.Find("HUD").transform.Find("PlayerNotify").GetComponent<TMP_Text>();
        _killCountText = _canvas.transform.Find("HUD").transform.Find("KillCountText").GetComponent<TMP_Text>();
        _enemiesRemainingText = _canvas.transform.Find("HUD").transform.Find("EnemiesRemainingText").GetComponent<TMP_Text>();
        _playerNameDisplay = _canvas.transform.Find("HUD").transform.Find("PlayerName").GetComponent<TMP_Text>();

        // maybe try this after
        _senseSlider.onValueChanged.AddListener(delegate { _senseSliderVal.text = ((int)(100 * _senseSlider.value)).ToString() + " %"; });

        ResetHUDisplay(); // clear HUD display values

    }

    void Start()
    {
        GameManager.Instance.isActive = false; // set to false to not allow player input until game starts
        Debug.Log("UI Manager Start");
        StartUI(_startState);
    }

    // UI state management methods ---------------------------------------------------------------------------------------------------

    private void StartUI(UIState state)
    {
        if (_currentState != _previousState) _previousState = _currentState;

        // disable all child canvases
        foreach (Transform child in _canvas.transform)
        {
            Canvas childCanvas = child.GetComponent<Canvas>();
            if (child != null) childCanvas.enabled = false;
        }

        // enable the selected canvas
        Canvas selectedCanvas = GetCanvasForState(state);
        if (selectedCanvas != null)
        {
            selectedCanvas.enabled = true;
            _currentState = state;
            Debug.Log("Started UI: " + state.ToString());
        }

        // unlock/lock cursor depending on selected canvas
        if (_currentState != UIState.HUD)
        {
            //unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    private Canvas GetCanvasForState(UIState state)
    {
        string canvasName = state.ToString();
        Transform selectedCanvasTransform = _canvas.transform.Find(canvasName);

        if (selectedCanvasTransform != null)
        {
            return selectedCanvasTransform.GetComponent<Canvas>();
        }

        return null;
    }

    private void ApplySettings(string name, float senseVal)
    {
        UserSettings.PlayerName = name;
        UserSettings.MouseSensitivity = senseVal;

        UpdatePlayerNameDisplay(name);

        //_nameText.text = !string.IsNullOrEmpty(name) ? name : "Guest User";
        _senseSliderVal.text = " :     " + ((int)(senseVal * 100)).ToString() + "%"; //get percent

        StartUI(_previousState);

        // After screen has deactivated, update the placeholder name in settings
        _placeHolderText.text = !string.IsNullOrEmpty(name) ? name : "Yurname...";
    }


    // public methods for UI state management ---------------------------------------------------------------------------------------------------

    public void PlayMain() => StartUI(UIState.Main);
    public void PlayPrev() => StartUI(_previousState);
    public void PlaySettings()
    {
        _senseSlider.value = UserSettings.MouseSensitivity; // set slider to current saved value
        StartUI(UIState.Settings);
    }
    public void PlayCredits() => StartUI(UIState.Credits);
    public void ConfirmSettings() => ApplySettings(_nameInput.text, _senseSlider.value);
    public void StartFadeOut() => StartCoroutine(nameof(FadeOut));

    public void PlayerPause()
    {
        Debug.Log("Player Pause");
        UserSettings.IsPaused = true;
        GameManager.Instance.isPaused = true;
        GameManager.Instance.isActive = false; //dont do this anywhere else

        StartUI(UIState.Pause);
    }

    public void QuitGame()
    {
        _gameManager.ResetGameState();
        Debug.Log("Retun to Main Menu");
        // set start text back to default
        _canvas.transform.Find("Main").transform.Find("StartButton").transform.Find("Text").GetComponent<TMP_Text>().text = "PLAY NOW";
        StartUI(UIState.Main);
    }

    public void PlayerUnPause()
    {
        Debug.Log("Player Resume");
        UserSettings.IsPaused = false;
        GameManager.Instance.isPaused = false;
        StartUI(UIState.HUD);
    }

    public void PlayAgain()
    {
        Debug.Log("UIManager Playing Again");
        PlayMain();
        _canvas.transform.Find("Main").transform.Find("StartButton").transform.Find("Text").GetComponent<TMP_Text>().text = "Play Again";

    }

    public void PlayGame()
    {
        StartUI(UIState.HUD);
        // slight delay to avoid immediate shoot
        Invoke(nameof(StartGame), 0.25f);
    }

    // encapsulates game manager StartGame() to be called after delay
    private void StartGame() => _gameManager.StartGame();

    public void ResetHUDisplay()
    {
        _timeText.text = "";
        _playerHealth.value = 100;
        _heatBar.value = 50;
        _waveCountText.text = "";
        _playerNotifyText.text = "";
        _killCountText.text = "";
        _enemiesRemainingText.text = "";
    }

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

    public void UpdatePlayerNameDisplay(string playerName)
    {
        _playerNameDisplay.text = playerName;
    }
    public void UpdateKillCount(int killCount)
    {
        _killCountText.text = "Kills: " + killCount;
    }

    public void UpdateEnemiesRemaining(int enemiesRemaining)
    {
        _enemiesRemainingText.text = "Enemies: " + enemiesRemaining;
    }

    public void UpdatePlayerNotifyText(string message)
    {
        _playerNotifyText.text = message;
    }

    // fade out loot text  ---------------------------------------------------------------------------------------------------
    public IEnumerator FadeOut()
    {
        //these dont exist and will throw an error if called - working on it
        var _incrementText = _canvas.transform.Find("HUD").transform.Find("IncrementText").GetComponent<TMP_Text>();
        var _lootText = _canvas.transform.Find("HUD").transform.Find("LootText").GetComponent<TMP_Text>();

        fadeSec = textFadeOutSeconds;
        while (fadeSec > 2f)
        {
            if (_incrementText != null && _lootText != null)
            {
                float alpha = fadeSec / textFadeOutSeconds;
                _incrementText.color = new Color(_incrementText.color.r, _incrementText.color.g, _incrementText.color.b, alpha);
                _lootText.color = new Color(_lootText.color.r, _lootText.color.g, _lootText.color.b, alpha);
            }
            fadeSec -= Time.deltaTime;
            yield return null;
        }

        // remove text and reset alpha
        _incrementText.text = "";
        _lootText.text = "";
        _incrementText.color = new Color(_incrementText.color.r, _incrementText.color.g, _incrementText.color.b);
        _lootText.color = new Color(_lootText.color.r, _lootText.color.g, _lootText.color.b);
    }


    // Update loop ---------------------------------------------------------------------------------------------------

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _gameManager.CurrentState.gameStarted)
        {
            if (!UserSettings.IsPaused) PlayerPause();
            else PlayerUnPause();
        }
    }

}
