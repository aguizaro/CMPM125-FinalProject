using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

//this class will be used to pass parameters to the wave generator
public struct WaveParams
{
    public int enemyCount;
    public float enemySpeed;
    public Vector3 location;
    public float radius;
    public int enemyHealth;
    public int enemyStrength;
}

public struct GameState
{
    public bool gameStarted;
    public int waveNumber;
    public int playerKillCount;
    public float playerHealth;
    public float playerHeat;
    public int enemiesRemaining;
    public bool timeisRunning; // is the timer running - will be true in between waves
    public float timeRemaining; //time remaining before next wave
    public int swordDamage;
    public int explosionDamage;
    public bool playerAttacked;
    public WaveParams currentWaveParams;
}

public class GameManager : MonoBehaviour
{
    // Perserving Jack's Singleton pattern --------------------------------------------------------------------------------------------------------
    private static GameManager _instance;
    private void Awake()
    {
        _instance = this;
    }
    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("Game Manager is NULL");
            }
            return _instance;
        }
    }
    public bool playerAttacked { get; set; }
    public int playerKillCount { get; set; }
    public bool hot { get; set; }
    public bool attack { get; set; }
    public bool isPaused { get; set; }
    public bool dashing { get; set; }
    public bool movable { get; set; }
    public bool explosive { get; set; }
    public float heat { get; set; }
    public bool hit { get; set; }
    public bool isActive { get; set; }
    public bool isDashing { get; set; }

    // Game State Variables ---------------------------------------------------------------------------------------------------------------------------
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private GameObject _playerInstance;
    [SerializeField] private float playerKnockBack = 50f;
    [SerializeField] private float playerHealthRegenAmount = 0.017f;
    [SerializeField] private GameObject _forge;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private float timeBetweenWaves = 20f;
    private Vector3[] _possibleForgePositions = { new Vector3(-132f, -0.13f, -17f), new Vector3(130f, -0.13f, -98.5f), new Vector3(81f, -0.13f, 158f), new Vector3(-5f, -0.13f, -185f), new Vector3(-84f, -0.13f, 243f), new Vector3(-174f, -0.13f, -230f) };
    public GameState CurrentState = new GameState();

    // Audio
    private AudioSource audioSource;
    public AudioClip fullHeatSound;
    public AudioClip playerHitSound;  // Added audio clip for player hit sound
    public AudioClip swordSwingSound;
    public AudioClip fullHealthSound; // Add this for the full health sound effect
    public AudioClip DashSound;


    private bool fullHealthSoundPlayed = true;




    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource if it's missing
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        GameManager.Instance.heat = 100f;
        ResetGameState();
        _arrow.SetActive(false);
    }


    // Spawning Waves/Enemies --------------------------------------------------------------------------------------------

    public void StartGame()
    {
        if (CurrentState.gameStarted) return; //if game is already started, don't start it again

        _playerInstance.SetActive(true);
        _UIManager.UpdatePlayerNotifyText("");
        ResetGameState();

        CurrentState.gameStarted = true;
        GameManager.Instance.isActive = true;
        GameManager.Instance.heat = 100f;
        NextWave();
    }

    // spawn enemies along the circumference of the circle defined by the waveParams.radius
    private void SpawnEnemy(WaveParams waveParams)
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 spawnPos = new Vector3(Mathf.Cos(randomAngle) * waveParams.radius, 0f, Mathf.Sin(randomAngle) * waveParams.radius);

        GameObject Enemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity, null);

        Enemy.gameObject.GetComponent<EnemyWander>().speed = waveParams.enemySpeed;
        Enemy.gameObject.GetComponent<EnemyWander>().health = waveParams.enemyHealth;
        Enemy.gameObject.GetComponent<EnemyWander>().strength = waveParams.enemyStrength;
        Enemy.gameObject.GetComponent<EnemyWander>().swordDamage = CurrentState.swordDamage;
    }
    public void SpawnWave(WaveParams waveParams)
    {
        for (int i = 0; i < waveParams.enemyCount; i++)
            SpawnEnemy(waveParams);

        Debug.Log("Wave " + CurrentState.waveNumber + " spawned\n Enemy count: " + waveParams.enemyCount + "\n Enemy speed: " + waveParams.enemySpeed + "\n Enemy health: " + waveParams.enemyHealth + "\n Spawn radius: " + waveParams.radius + "\n Location: " + waveParams.location);
    }

    public void NextWave()
    {
        CurrentState.waveNumber++;
        _UIManager.UpdateWaveDisplay(CurrentState.waveNumber);
        _UIManager.UpdateTimeDisplay(-1); //clear the time display


        //Spawn forge in random location
        int randomIndex = Random.Range(0, _possibleForgePositions.Length);
        _forge.transform.position = _possibleForgePositions[randomIndex];

        // define wave parameters and spawn wave
        WaveParams waveParams = GetWaveParams(CurrentState.waveNumber);
        SpawnWave(waveParams);

        CurrentState.currentWaveParams = waveParams;
        CurrentState.enemiesRemaining = waveParams.enemyCount;
        CurrentState.explosionDamage = 10;

        _UIManager.UpdateEnemiesRemaining(CurrentState.currentWaveParams.enemyCount);
        _UIManager.UpdateKillCount(CurrentState.playerKillCount);

    }

    //defines wave parameters based on wave number
    private WaveParams GetWaveParams(int waveNumber)
    {
        WaveParams waveParams = new WaveParams();
        waveParams.enemyCount = (waveNumber <= 2) ? waveNumber + 1 : waveNumber * 2; // 2, 3, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26
        waveParams.enemySpeed = 2f + waveNumber * 0.10f; // 10% increase in speed per wave
        waveParams.location = transform.position; // spawn location
        waveParams.radius = 40f + waveNumber * 2f; // 2m increase in enemy spawn radius per wave
        waveParams.enemyHealth = (waveNumber <= 2) ? 1 : 2; // 1 health for first 3 waves, 2 health for all subsequent waves
        waveParams.enemyStrength = 2;
        return waveParams;
    }


    // Check current state every frame ----------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        // always update the game state
        CurrentState.playerHeat = GameManager.Instance.heat;
        _UIManager.UpdateHealthDisplay(CurrentState.playerHealth);
        _UIManager.UpdateHeatDisplay(CurrentState.playerHeat);

        if (!CurrentState.gameStarted || GameManager.Instance.isPaused) GameManager.Instance.isActive = false;
        else GameManager.Instance.isActive = true;


        if (!GameManager.Instance.isActive) return; //if game is paused or ended, don't update the state

        // handle player attacked by enemy
        if (CurrentState.playerAttacked)
        {
            CurrentState.playerAttacked = false;
            CurrentState.playerHealth -= 2;

            audioSource.PlayOneShot(playerHitSound);


            // maybe add a force to knock the player back
            //_playerInstance.GetComponent<Rigidbody>().AddForce(-_playerInstance.transform.forward * playerKnockBack, ForceMode.Impulse);
            //Debug.Log("Player knocked back");

        }

        if (CurrentState.playerHeat <= 20 && !_arrow.activeSelf)
        {
            _arrow.SetActive(true);
            Debug.Log("Player health is low");
        }

        if (CurrentState.playerHeat > 20 && _arrow.activeSelf)
        {
            _arrow.SetActive(false);
            Debug.Log("Player health is back to normal");
        }

        // handle player death
        if (CurrentState.playerHealth <= 0)
        {
            CurrentState.gameStarted = false;
            DestroyRemainingEnemies();

            _playerInstance.SetActive(false); //deactivate player


            Debug.Log("Game Over - Player died");
            _UIManager.UpdatePlayerNotifyText("Game Over!");
            Invoke(nameof(GameOver), 3f); //restart the game after 3 seconds

            //
            //end the game and go back to ui main menu
            //
        }
        else if (CurrentState.playerHealth < 100 && CurrentState.playerHealth > 0)
        {
            CurrentState.playerHealth += playerHealthRegenAmount; //very slow health regen on player
        }

        if (CurrentState.timeisRunning && Input.GetKeyDown(KeyCode.Return))
        {
            StopAllCoroutines();
            CurrentState.timeisRunning = false;
            _UIManager.UpdatePlayerNotifyText("");
            NextWave();
        }

        // handle wave completion
        if (CurrentState.enemiesRemaining <= 0 && !CurrentState.timeisRunning)
        {
            CurrentState.timeisRunning = true;
            CurrentState.timeRemaining = timeBetweenWaves;
            Debug.Log("Wave " + CurrentState.waveNumber + " completed");
            StartCoroutine(StartTimer());
            _UIManager.UpdatePlayerNotifyText("Press [return] to start next wave");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && (GameManager.Instance.heat >= 5.0f))
        {
            audioSource.PlayOneShot(swordSwingSound);
        }

        if (CurrentState.playerHealth >= 100.0f && !fullHealthSoundPlayed)
        {
            audioSource.PlayOneShot(fullHealthSound);
            fullHealthSoundPlayed = true; // Prevent the sound from being played again while health is full
        }
        else if (CurrentState.playerHealth < 100.0f)
        {
            fullHealthSoundPlayed = false; // Allow the sound to be played again if health drops below full
        }

        if (isDashing)
        {
            Debug.Log("in dash sound loop: false");
            isDashing = false;
            audioSource.PlayOneShot(DashSound);
        }

        GameManager.Instance.playerKillCount = CurrentState.playerKillCount;

    }

    // reset game state and activate play again menu
    private void GameOver()
    {
        ResetGameState();
        Debug.Log("Calling UIManager PlayAgain ");
        _UIManager.PlayAgain();
    }

    public void ResetGameState()
    {
        CurrentState.gameStarted = false;
        CurrentState.waveNumber = 0;
        CurrentState.playerKillCount = 0;
        CurrentState.playerHealth = 100;
        CurrentState.playerHeat = 100f;
        CurrentState.enemiesRemaining = 99;
        CurrentState.timeisRunning = false;
        CurrentState.timeRemaining = 0f;
        CurrentState.swordDamage = 2;
        CurrentState.explosionDamage = 10;
        CurrentState.playerAttacked = false;
        CurrentState.currentWaveParams = new WaveParams();

        GameManager.Instance.isPaused = false;
        _playerInstance.transform.position = new Vector3(0, 1.03f, 0);
        _playerInstance.transform.rotation = Quaternion.identity;

        _UIManager.ResetHUDisplay();

        DestroyRemainingEnemies();
    }

    // clears remaining enemies when game ends
    private void DestroyRemainingEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    // -----------------------------------------------------------------------------------------------------------------------------------------------------------
    // Begin countdown and then start the next wave
    IEnumerator StartTimer()
    {
        _UIManager.UpdateWaveDisplay(-1); //display notice that next wave is coming
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // Wait for .1 second
            if (!isPaused && CurrentState.timeisRunning)
            {
                if (CurrentState.timeRemaining > 0f)
                {
                    CurrentState.timeRemaining -= 0.1f;
                    _UIManager.UpdateTimeDisplay(CurrentState.timeRemaining);

                    //maybe play a ticking sound here for the clock
                }
                else
                {
                    CurrentState.timeisRunning = false;
                    NextWave();
                    break;

                }

            }
        }
    }


    // Temp control ------------------------------------------------------------------------------------------------------------------------------------------------

    public void IncreaseTemperature(float amount)
    {
        int maxHeat = 100;
        if (GameManager.Instance.heat >= maxHeat) return;

        GameManager.Instance.heat += amount;
        GameManager.Instance.heat = Mathf.Min(GameManager.Instance.heat, maxHeat);

        if (GameManager.Instance.heat == maxHeat)
        {
            //play hothothot
            audioSource.PlayOneShot(fullHeatSound);
        }
    }

    public void DecreaseTemperature(float amount)
    {
        GameManager.Instance.heat -= amount;
        GameManager.Instance.heat = Mathf.Max(GameManager.Instance.heat, 0);
        if (GameManager.Instance.heat == 0)
        {
            //play cold SFX or display some visual effect
        }
    }
}
