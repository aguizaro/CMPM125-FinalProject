using System.Collections;
using UnityEngine;

public class EnemySnowmanShooting : MonoBehaviour
{
    private Transform player;
    public GameObject projectilePrefab;
    public float xOffset = 1f;
    public float yOffset = -1f;
    public float shootingInterval = 1f;
    public float projectileSpeed = 10f;
    public float maxShootDistance = 10f; // Maximum distance to shoot at the player

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootingInterval);

            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer <= maxShootDistance)
                {
                    Vector3 direction = (player.position - transform.position).normalized;
                    Vector3 spawnPosition = transform.position + direction * xOffset + Vector3.up * yOffset;
                    GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
                    StartCoroutine(DestroyProjectile(projectile));
                    Rigidbody rb = projectile.GetComponent<Rigidbody>();
                    rb.velocity = direction * projectileSpeed;
                }
            }
        }
    }

    IEnumerator DestroyProjectile(GameObject projectile)
    {
        yield return new WaitForSeconds(2f);
        Destroy(projectile);
    }
}
