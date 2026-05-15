using TMPro;
using UnityEngine;

public class MainMenuHighScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;

    void Start()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            highScoreText.gameObject.SetActive(false);
            return;
        }

        highScoreText.gameObject.SetActive(true);
        highScoreText.text = "Highest Score: " + PlayerPrefs.GetInt("HighScore") + " m";
    }
}