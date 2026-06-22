using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuStuff : MonoBehaviour
{

    [SerializeField]Button StartButton;
    [SerializeField]Button ExitButton;
    [SerializeField] Button AchievementsButton;

    [SerializeField] TMP_Text highScore;
    [SerializeField] TMP_Text finalScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartButton.onClick.AddListener(StartGame);
        ExitButton.onClick.AddListener(ExitGame);
        AchievementsButton.onClick.AddListener(ShowAchievements);

        highScore = GameManager.highScore;
        highScore.text = GameManager.highScore.text;

        finalScore = GameManager.finalScore;
        finalScore.text = GameManager.finalScore.text;

        highScore.gameObject.SetActive(false);
        finalScore.gameObject.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void ShowAchievements()
    {
        highScore.gameObject.SetActive(true);
        finalScore.gameObject.SetActive(true);
    }
}
