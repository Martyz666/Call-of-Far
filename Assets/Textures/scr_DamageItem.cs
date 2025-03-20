using UnityEngine;

public class scr_DamageItem : MonoBehaviour
{
    #region - OnTrigger -

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<scr_PlayerStats>().Die();
        }
    }

    #endregion
}