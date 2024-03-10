using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForgeTempController : MonoBehaviour
{
    [SerializeField] private float forgeIncreateRate = 5f; // Temperature increase rate per second in forge area    

    private GameManager _gameManager;
    //private UIManager _UIManager;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //_UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _gameManager.IncreaseTemperature(forgeIncreateRate * Time.deltaTime); // Gradually increase temperature in forge
        }
    }

}
