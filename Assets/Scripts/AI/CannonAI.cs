using UnityEngine;
using System.Collections;

public class CannonAI : MonoBehaviour
{
    public Transform player;
    public Transform firePoint;
    public GameObject cannonBallPrefab;
    public GameObject destructionEffectPrefab;
    public float range = 20f;
    public float fireRate = 1f;
    public float rotationSpeed = 5f;
    public AudioClip fireSound;

    private AudioSource audioSource;
    private float fireCooldown = 0f;
    private int health = 100;

    #region - Start Update -

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!scr_PlayerStats.isDead && !PauseMenu.isPaused)
        {
            if (player == null)
            {
                Debug.LogWarning("Player object is not assigned!");
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= range && CanSeePlayer())
            {
                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

                if (fireCooldown <= 0f)
                {
                    Shoot();
                    fireCooldown = 1f / fireRate;
                }
            }

            if (fireCooldown > 0f)
            {
                fireCooldown -= Time.deltaTime;
            }
        }
    }

    #endregion

    #region - Visibility Check -

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        float distanceToPlayer = Vector3.Distance(firePoint.position, player.position);

        if (Physics.Raycast(firePoint.position, directionToPlayer, out RaycastHit hit, distanceToPlayer))
        {
            if (hit.transform == player)
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    #region - Take Damage -

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Cannon hit! HP: " + health);

        if (health <= 0)
        {
            StartCoroutine(DestroyCannon());
        }
    }

    private IEnumerator DestroyCannon()
    {
        GameObject effectInstance = null;
        if (destructionEffectPrefab != null)
        {
            effectInstance = Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
        Debug.Log("Cannon destroyed!");

        if (effectInstance != null)
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(effectInstance);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(25);
            Destroy(collision.gameObject);
        }
    }

    #endregion

    #region - Shoot -

    void Shoot()
    {
        GameObject cannonBall = Instantiate(cannonBallPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * 1500f);
        }

        Destroy(cannonBall, 5f);

        if (fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

    #endregion

    #region - Gizmos -

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    #endregion
}