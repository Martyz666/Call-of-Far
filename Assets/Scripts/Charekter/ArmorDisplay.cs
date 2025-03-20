using UnityEngine;
using TMPro;

public class ArmorDisplay : MonoBehaviour
{
    public PlayerArmor playerArmor;
    public TextMeshProUGUI armorText;

    private void Start()
    {
        if (playerArmor != null)
        {
            playerArmor.onArmorChanged.AddListener(UpdateArmorUI);
            UpdateArmorUI(playerArmor.GetCurrentArmor());
        }
    }

    private void UpdateArmorUI(int armor)
    {
        armorText.text = "" + armor.ToString();
    }
}