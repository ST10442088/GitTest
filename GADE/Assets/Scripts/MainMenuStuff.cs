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

        highScore.gameObject.SetActive(false);
        finalScore.gameObject.SetActive(false);

        highScore.text = "Your High Score: "+PlayerPrefs.GetInt("SavedHighScore").ToString();
        finalScore.text = "Your Previous Score: " + PlayerPrefs.GetString("Final Score");
        
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
