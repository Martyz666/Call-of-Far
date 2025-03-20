using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip scoreSound;

    private int currentScore = 0;

    #region - Awake Start -

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    #endregion

    #region - Score -

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreUI();
        PlayScoreSound();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "" + currentScore;
        }
    }

    private void PlayScoreSound()
    {
        if (audioSource != null && scoreSound != null)
        {
            audioSource.PlayOneShot(scoreSound);
        }
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    #endregion
}