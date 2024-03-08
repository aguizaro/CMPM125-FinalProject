using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Slider healthBar; // Assign the UI Slider in the inspector
    public float health = 1; // Initial health
    public float regenerationRate = 1; // Health points regenerated per second

    private void Start()
    {
        // Initialize health bar value
        healthBar.value = health;
    }

    private void Update()
    {
        // Regenerate health if it is below 100
        if (health < 100)
        {
            health += regenerationRate * Time.deltaTime;
            health = Mathf.Min(health, 100); // Ensure health doesn't exceed 100
            healthBar.value = health;
        }

        // Check if health is depleted
        if (health <= 0)
        {
            // Restart the game. Make sure you have your scene added in the build settings
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Example method to deal damage
    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Max(health, 0); // Ensure health doesn't go below 0
        healthBar.value = health;
    }
}
