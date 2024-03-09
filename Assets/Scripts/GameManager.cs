using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
}

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameManager _instance;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private UIManager _UIManager;

    private float timeElapsed = 0f;
    private bool isPaused = false;
    private int currentWave = 0;


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
    private void Awake()
    {
        _instance = this;
    }
    public bool hit { get; set; }
    public bool hot { get; set; }

    public bool attack { get; set; }

    public bool dashing { get; set; }
    public bool movable { get; set; }

    /// <summary>
    public bool explosive { get; set; }
    /// </summary>
    
    public float heat { get; set; }

    // spawn enemies along the circumference of the circle defined by the waveParams.radius
    private void SpawnEnemy(WaveParams waveParams)
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 spawnPos = new Vector3(Mathf.Cos(randomAngle) * waveParams.radius, 0f, Mathf.Sin(randomAngle) * waveParams.radius);

        Debug.Log("spawn pos: " + spawnPos);
        GameObject Enemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity, null);

        Enemy.gameObject.GetComponent<EnemyWander>().speed = waveParams.enemySpeed;
        Enemy.gameObject.GetComponent<EnemyDamage>().health = waveParams.enemyHealth;

    }

    public void SpawnWave(WaveParams waveParams)
    {
        for (int i=0; i<waveParams.enemyCount; i++)
        {
            SpawnEnemy(waveParams);
        }

    }

    private void Start()
    {
        currentWave = 1;
        _UIManager.UpdateWaveDisplay(currentWave);

        //define player spawn point - not done

        // define forge count and spawn points will be randomized - not done

        WaveParams waveParams = new WaveParams { location = new Vector3(0f, 1f, 0f), enemyCount = 20, enemyHealth = 1, enemySpeed = 2, radius = 50 };

        SpawnWave(waveParams);

        StartCoroutine(StartTimer());


    }


    IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            if (!isPaused)
            {
                timeElapsed += 1f;
                _UIManager.UpdateTimeDisplay(timeElapsed);
            }
        }
    }
}
