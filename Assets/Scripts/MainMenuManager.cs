using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;

    void Start()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);
    }

    public void StartGame()
    {
        PlayButtonSound();
        SceneManager.LoadScene("Main");
    }

    public void OpenInstructions()
    {
        PlayButtonSound();

        if (instructionsPanel != null)
            instructionsPanel.SetActive(true);
    }

    public void CloseInstructions()
    {
        PlayButtonSound();

        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        PlayButtonSound();
        Application.Quit();
    }

    private void PlayButtonSound()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }
}