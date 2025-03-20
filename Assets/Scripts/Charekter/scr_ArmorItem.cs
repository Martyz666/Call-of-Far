using UnityEngine;

public class scr_ArmorItem : MonoBehaviour
{
    public int armorAmount;
    public AudioClip pickupSound;
    public Vector3 rotationSpeed = new Vector3(0, 50, 0);

    #region - Update -

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region - OnTrigger -

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerArmor playerArmor = other.GetComponent<PlayerArmor>();

            if (playerArmor != null)
            {
                playerArmor.RestoreArmor(armorAmount);

                if (pickupSound != null)
                {
                    PlaySoundAndDestroy(pickupSound);
                }

                Destroy(gameObject);
            }
        }
    }

    #endregion

    #region - PlaySoundAndDestroy -

    private void PlaySoundAndDestroy(AudioClip clip)
    {
        GameObject soundObject = new GameObject("PickupSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.playOnAwake = false;

        audioSource.Play();

        Destroy(soundObject, clip.length);
    }

    #endregion
}