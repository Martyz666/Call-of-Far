using UnityEngine;
using UnityEngine.UI;

public class scr_HealthBar : MonoBehaviour
{
    public Slider healthSlider;

    #region - SetSlider -

    public void SetSlider(float amount)
    {
        healthSlider.value = amount;
    }

    public void SetSliderMax(float amount)
    {
        healthSlider.maxValue = amount;
        SetSlider(amount);
    }

    #endregion
}