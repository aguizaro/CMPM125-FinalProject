using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotColdBarControl : MonoBehaviour
{
    [SerializeField] private float maxTemperature = 100f; // Maximum temperature value
    private float currentTemperature = 20f; // Current temperature
    public Slider temperatureSlider; // The UI slider to represent the temperature
    public TextMeshProUGUI temperatureText; // The text element that shows "HOT" or "COLD"
    [SerializeField] private float increaseRate = 5f; // Temperature increase rate per second in forge area
    [SerializeField] private float decreaseRate = 10f; // Temperature decrease rate on click
    public AudioClip fullHeatSound; // Sound to play when the temperature bar is full
    private AudioSource audioSource; // AudioSource component to play the sound



    private void Start()
    {
        if (temperatureSlider == null)
        {
            temperatureSlider = GetComponent<Slider>();
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource if it's missing
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Initialize the UI to reflect the starting state
        UpdateTemperatureBar();
    }

    private void Update()
    {
        // Drain the heat bar when the left mouse button is clicked
        if (Input.GetMouseButtonDown(0)) // 0 is the button number for the left mouse button
        {
            DecreaseTemperature(decreaseRate); // Decrease temperature on click
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IncreaseTemperature(increaseRate * Time.deltaTime); // Gradually increase temperature in forge
        }
    }

    private void IncreaseTemperature(float amount)
    {
        if (currentTemperature < maxTemperature)
        {
            currentTemperature += amount;
            currentTemperature = Mathf.Min(currentTemperature, maxTemperature);
            UpdateTemperatureBar();

            if (currentTemperature >= maxTemperature)
            {
                // Play sound when the temperature bar is full
                audioSource.PlayOneShot(fullHeatSound);
            }
        }
    }

    private void DecreaseTemperature(float amount)
    {
        currentTemperature -= amount;
        currentTemperature = Mathf.Max(currentTemperature, 0);
        UpdateTemperatureBar();
    }

    private void UpdateTemperatureBar()
    {
        temperatureSlider.value = currentTemperature;

        if (currentTemperature > maxTemperature * 0.5)
        {
            //temperatureSlider.fillRect.GetComponentInChildren<Image>().color = Color.red;
            temperatureText.text = "HOT";
            temperatureText.color = Color.red;
        }
        else
        {
           // temperatureSlider.fillRect.GetComponentInChildren<Image>().color = Color.blue;
            temperatureText.text = "COLD";
            temperatureText.color = Color.blue;
        }
    }
}
