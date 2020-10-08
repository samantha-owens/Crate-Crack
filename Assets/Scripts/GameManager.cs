using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public GameObject titleScreen;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI easyHighScoreText;
    public TextMeshProUGUI mediumHighScoreText;
    public TextMeshProUGUI hardHighScoreText;
    public TextMeshProUGUI newHighScoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    public int score;
    public int easyHighScore;
    public int mediumHighScore;
    public int hardHighScore;

    private float spawnRate = 2.0f;

    public bool isGameActive;
    public bool easyMode = false;
    public bool mediumMode = false;
    public bool hardMode = false;

    void Start()
    {
        // grabs saved high scores for each mode
        easyHighScore = PlayerPrefs.GetInt("easyhighscore", 0);
        mediumHighScore = PlayerPrefs.GetInt("mediumhighscore", 0);
        hardHighScore = PlayerPrefs.GetInt("hardhighscore", 0);

        // updates high score UIs
        easyHighScoreText.text = "Easy: " + easyHighScore;
        mediumHighScoreText.text = "Medium: " + mediumHighScore;
        hardHighScoreText.text = "Hard: " + hardHighScore;
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        score = 0;
        scoreText.gameObject.SetActive(true);

        // divide the spawn rate by the difficulty so the items will spawn faster with a higher difficulty
        spawnRate /= difficulty;

        StartCoroutine(SpawnTarget());
        UpdateScore(0);
    }

    // while the game is active, spawn random targets at the level's spawn rate
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    // add or subtract points to the score based on the target hit and update UI
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    // save high scores for each difficulty and logs them to UI
    public void SaveScores()
    {
        if (easyMode && score > easyHighScore)
        {
            PlayerPrefs.SetInt("easyhighscore", score);
            easyHighScoreText.text = "Easy: " + score;
            easyHighScoreText.GetComponent<TextMeshProUGUI>().color = Color.red;
            StartCoroutine(FlashText());
        }
        else if (mediumMode && score > mediumHighScore)
        {
            PlayerPrefs.SetInt("mediumhighscore", score);
            mediumHighScoreText.text = "Medium: " + score;
            mediumHighScoreText.GetComponent<TextMeshProUGUI>().color = Color.red;
            StartCoroutine(FlashText());
        }
        else if (hardMode && score > hardHighScore)
        {
            PlayerPrefs.SetInt("hardhighscore", score);
            hardHighScoreText.text = "Hard: " + score;
            hardHighScoreText.GetComponent<TextMeshProUGUI>().color = Color.red;
            StartCoroutine(FlashText());
        }

        PlayerPrefs.Save();
    }

    // flash the high score text
    public IEnumerator FlashText()
    {
        while (!isGameActive)
        {
            newHighScoreText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            newHighScoreText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    // makes game inactive, activates game over text and restart button, saves high scores
    public void GameOver()
    {
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        SaveScores();
    }

    // restarts the scene, attached to the RESTART button in UI
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
