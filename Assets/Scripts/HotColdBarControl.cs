using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotColdBarControl : MonoBehaviour
{
    [SerializeField] private float maxTemperature = 100f; // Maximum temperature value
    private float currentTemperature = 20f; // Current temperature
    public Image temperatureBarFill; // The UI element that represents the temperature fill
    public TextMeshProUGUI temperatureText; // The text element that shows "HOT" or "COLD"
    private bool isPlayerInForge = false; // Flag to check if the player is in the forge
    [SerializeField] private float temperatureIncreaseRate = 5f; // Rate of temperature increase per second

    [SerializeField] private AudioSource audioSource; // Reference to the audio source component
    [SerializeField] private AudioClip hotHotHotClip; // Reference to the audio clip

    private void Start()
    {
        GameManager.Instance.hit = false;
        UpdateTemperatureBar();
    }

    private void Update()
    {
        // Increase temperature if player is in the forge area
        if (isPlayerInForge)
        {
            IncreaseTemperature(temperatureIncreaseRate * Time.deltaTime);
        }

        // Check for player attack (left mouse click) and decrease temperature
        if (Input.GetMouseButtonDown(0))
        {
            DecreaseTemperature(10.0f);
        }

        // Play the sound effect when the bar is full and the clip is not null
        if (currentTemperature >= maxTemperature && hotHotHotClip != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(hotHotHotClip);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInForge = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInForge = false;
        }
    }

    private void IncreaseTemperature(float amount)
    {
        currentTemperature += amount;
        currentTemperature = Mathf.Min(currentTemperature, maxTemperature);
        UpdateTemperatureBar();
    }

    private void DecreaseTemperature(float amount)
    {
        currentTemperature -= amount;
        currentTemperature = Mathf.Max(currentTemperature, 0);
        UpdateTemperatureBar();
    }

    private void UpdateTemperatureBar()
    {
        float fillAmount = currentTemperature / maxTemperature;
        temperatureBarFill.fillAmount = fillAmount;

        if (currentTemperature > maxTemperature * 0.5)
        {
            temperatureBarFill.color = Color.red;
            temperatureText.text = "HOT";
            temperatureText.color = Color.red;
        }
        else
        {
            temperatureBarFill.color = Color.blue;
            temperatureText.text = "COLD";
            temperatureText.color = Color.blue;
        }
    }
}
