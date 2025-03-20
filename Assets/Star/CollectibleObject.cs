using UnityEngine;
using TMPro;

public class CollectibleObject : MonoBehaviour
{
    public static bool isWin;

    [Header("UI Elements")]
    public TextMeshProUGUI warningText;
    public GameObject actionButtonsUI;

    [Header("Settings")]
    public int requiredScore = 10;

    public Vector3 rotationSpeed = new Vector3(0, 50, 0);

    #region - Start -

    private void Start()
    {
        Time.timeScale = 1f;
        isWin = false;

        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }

        if (actionButtonsUI != null)
        {
            actionButtonsUI.SetActive(false);
        }
    }

    #endregion

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
            if (ScoreManager.Instance != null && ScoreManager.Instance.GetCurrentScore() >= requiredScore)
            {
                CollectObject();
                isWin = true;
            }
            else
            {
                ShowWarning();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideWarning();
        }
    }

    #endregion

    #region - CollectObject -

    private void CollectObject()
    {
        if (actionButtonsUI != null)
        {
            actionButtonsUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        gameObject.SetActive(false);
    }

    #endregion

    #region - Warning -

    private void ShowWarning()
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true);
            warningText.text = "You need 10 points to collect this item!";
        }
    }

    private void HideWarning()
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }
    }

    #endregion
}