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
    public bool gamePaused;
    public bool gameEnded;
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

    // Game State Variables ---------------------------------------------------------------------------------------------------------------------------
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _forge;
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private float timeBetweenWaves = 10f;
    private Vector3[] _possibleForgePositions = { new Vector3(-132f, -0.13f, -17f), new Vector3(130f, -0.13f, -98.5f), new Vector3(81f, -0.13f, 158f), new Vector3(-5f, -0.13f, -185f), new Vector3(-84f, -0.13f, 243f), new Vector3(-174f, -0.13f, -230f) };
    public GameState CurrentState = new GameState() { gameStarted = false, gamePaused = false, gameEnded = false, waveNumber = 0, playerKillCount = 0, playerHealth = 100, playerHeat = 50f, enemiesRemaining = 99, timeisRunning = false, timeRemaining = 0f, swordDamage = 2, explosionDamage = 10, playerAttacked = false, currentWaveParams = new WaveParams() };

    // Audio
    private AudioSource audioSource;
    public AudioClip fullHeatSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource if it's missing
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        GameManager.Instance.heat = 100f;
    }


    // Spawning Waves/Enemies --------------------------------------------------------------------------------------------

    private void StartGame()
    {
        if (CurrentState.gameStarted) return;


        CurrentState.gameStarted = true;
        Debug.Log("Starting game");
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

        //Spawn forge in random location
        int randomIndex = Random.Range(0, _possibleForgePositions.Length);
        _forge.transform.position = _possibleForgePositions[randomIndex];
        Debug.Log("new forge position: " + _forge.transform.position);

        // define wave parameters and spawn wave
        WaveParams waveParams = GetWaveParams(CurrentState.waveNumber);
        SpawnWave(waveParams);

        CurrentState.currentWaveParams = waveParams;
        CurrentState.enemiesRemaining = waveParams.enemyCount;
        Debug.Log("Spawned " + CurrentState.enemiesRemaining + " enemies");

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
        return waveParams;
    }


    // Check current state every frame ----------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        // add this later
        //if (CurrentState.gameEnded) return;

        CurrentState.playerHeat = GameManager.Instance.heat;
        CurrentState.gamePaused = GameManager.Instance.isPaused;
        _UIManager.UpdateHealthDisplay(CurrentState.playerHealth);
        _UIManager.UpdateHeatDisplay(CurrentState.playerHeat);


        if (!CurrentState.gameStarted)
        {
            CurrentState.gameEnded = false;
            if (Input.GetKeyDown(KeyCode.Return)) StartGame();
            return;
        }

        if (CurrentState.playerAttacked)
        {
            CurrentState.playerAttacked = false;
            CurrentState.playerHealth -= 10;

            Debug.Log("Player took damage - health: " + CurrentState.playerHealth);

            //play enemy got hit sound
            // maybe add a force to knock the player back


        }

        if (CurrentState.playerHealth <= 0)
        {
            CurrentState.gameEnded = true;
            //
            //end the game and go back to ui main menu
            //
        }

        if (CurrentState.enemiesRemaining <= 0)
        {
            Debug.Log("Wave " + CurrentState.waveNumber + " completed");
            CurrentState.timeisRunning = true;
            StartCoroutine(StartTimer());
        }

        GameManager.Instance.playerKillCount = CurrentState.playerKillCount;



    }

    // -----------------------------------------------------------------------------------------------------------------------------------------------------------
    // Begin countdown and then start the next wave
    IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            if (!isPaused && CurrentState.timeisRunning)
            {
                if (CurrentState.timeRemaining > 0f)
                {
                    CurrentState.timeRemaining -= 1;
                    _UIManager.UpdateTimeDisplay(CurrentState.timeRemaining);

                    //maybe play a ticking sound here for the clock
                }
                else
                {
                    CurrentState.timeisRunning = false;
                    CurrentState.timeRemaining = timeBetweenWaves;
                    NextWave();
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
