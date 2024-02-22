using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotColdBarControl : MonoBehaviour
{
    [SerializeField] private float maxTemperature = 100f; // Maximum temperature value
    private float currentTemperature = 20f; // Current temperature, starts at 0
    public Image temperatureBarFill; // The UI element that represents the temperature fill
    public TextMeshProUGUI temperatureText; // The text element that shows "HOT" or "COLD"

    private void Start()
    {
        GameManager.Instance.hit = false;
        // Initialize the UI to reflect the starting state
        UpdateTemperatureBar();
    }
    private void Update()
    {
        if(GameManager.Instance.hit == true)
        {
            DecreaseTemperature(10.0f);
            GameManager.Instance.hit = false;
        }
    }
    //commiting
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            IncreaseTemperature(60f);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            DecreaseTemperature(60f);
        }
    }


    private void IncreaseTemperature(float amount)
    {
        currentTemperature += amount;
        currentTemperature = Mathf.Min(currentTemperature, maxTemperature); // Cap the temperature
        UpdateTemperatureBar();
    }

    private void DecreaseTemperature(float amount)
    {
        currentTemperature -= amount;
        currentTemperature = Mathf.Max(currentTemperature, 0); // Prevent going below 0
        UpdateTemperatureBar();
    }

    private void UpdateTemperatureBar()
    {
        // Calculate fill amount
        float fillAmount = currentTemperature / maxTemperature;
        temperatureBarFill.fillAmount = fillAmount;

        // Update the color and text based on the temperature
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
