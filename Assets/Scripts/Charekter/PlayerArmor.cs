using UnityEngine;
using UnityEngine.Events;

public class PlayerArmor : MonoBehaviour
{
    public int maxArmor = 100;
    private int currentArmor;

    public UnityEvent<int> onArmorChanged;

    #region - Start -

    private void Start()
    {
        currentArmor = maxArmor;
        onArmorChanged.Invoke(currentArmor);
    }

    #endregion

    #region - TakeDamage -

    public void TakeDamage(int damage)
    {
        currentArmor -= damage;
        currentArmor = Mathf.Clamp(currentArmor, 0, maxArmor);
        onArmorChanged.Invoke(currentArmor);
    }

    #endregion

    #region - Armor -

    public void RestoreArmor(int amount)
    {
        currentArmor += amount;
        currentArmor = Mathf.Clamp(currentArmor, 0, maxArmor);
        onArmorChanged.Invoke(currentArmor);
    }

    public int GetCurrentArmor()
    {
        return currentArmor;
    }

    #endregion

}