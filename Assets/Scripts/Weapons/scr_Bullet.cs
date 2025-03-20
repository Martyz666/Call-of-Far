using UnityEngine;

public class scr_Bullet : MonoBehaviour
{
    #region - Collision -

    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Target"))
        {
            Debug.Log("Hit " + objectWeHit.gameObject.name + " !");
            ScoreManager.Instance.AddScore(2);
            Destroy(objectWeHit.gameObject);
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit a wall");
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }

        CannonAI cannon = objectWeHit.gameObject.GetComponentInParent<CannonAI>();
        if (cannon != null)
        {
            cannon.TakeDamage(25);
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }
    }

    #endregion

    #region - BulletEffect -

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject hole = Instantiate(GobalReferences.Instance.bulletImpactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }

    #endregion
}