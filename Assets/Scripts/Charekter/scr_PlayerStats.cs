using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;

    public scr_HealthBar healthBar;
    public PlayerArmor playerArmor;

    [Header("UI Elements")]
    public GameObject deadScreenUI;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSound;

    public static bool isDead;

    #region - Start -

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        isDead = false;
        Time.timeScale = 1f;

        if (deadScreenUI != null)
        {
            deadScreenUI.SetActive(false);
        }
    }

    #endregion

    #region - TakeDamage -

    public void TakeDamage(float amount)
    {
        int damage = (int)amount;

        if (playerArmor != null && playerArmor.GetCurrentArmor() > 0)
        {
            playerArmor.TakeDamage(damage);
        }
        else
        {
            currentHealth -= damage;
            healthBar.SetSlider(currentHealth);
        }
    }

    #endregion

    #region - Heal -

    public void Heal(float amount)
    {
        if (!isDead)
        {
            currentHealth += amount;
            healthBar.SetSlider(currentHealth);

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    #endregion

    #region - Update -

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20f);
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    #endregion

    #region - Die -

    public void Die()
    {
        isDead = true;

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (deadScreenUI != null)
        {
            deadScreenUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isDead = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isDead = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    #endregion
}