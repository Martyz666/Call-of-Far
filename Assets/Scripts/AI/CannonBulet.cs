using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float damage = 20f;
    private AudioSource audioSource;

    #region - Start -

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    #endregion

    #region - Collision -

    private void OnCollisionEnter(Collision collision)
    {
        scr_PlayerStats playerStats = collision.gameObject.GetComponent<scr_PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);

            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
        Destroy(gameObject, 0.1f);
    }

    #endregion
}