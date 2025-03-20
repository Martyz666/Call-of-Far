using UnityEngine;
using UnityEngine.Audio;

public class Past : MonoBehaviour
{
    public float deadHit = 100f;

    #region - Collision -

    private void OnCollisionEnter(Collision collision)
    {
        scr_PlayerStats playerStats = collision.gameObject.GetComponent<scr_PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(deadHit);
        }
    }

    #endregion
}