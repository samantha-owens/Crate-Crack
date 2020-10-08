using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    private GameManager gameManager;
    private Button button;

    public int difficulty;

    // Start is called before the first frame update
    void Start()
    {
        // reference to the game manager script
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // reference to the difficulty buttons
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
    }

    // set difficulty based on which button is clicked
    void SetDifficulty()
    {
        gameManager.StartGame(difficulty);
        gameManager.titleScreen.gameObject.SetActive(false);

        if (button.gameObject.CompareTag("Easy"))
        {
            gameManager.easyMode = true;
            gameManager.mediumMode = false;
            gameManager.hardMode = false;
        }
        else if (button.gameObject.CompareTag("Medium"))
        {
            gameManager.easyMode = false;
            gameManager.mediumMode = true;
            gameManager.hardMode = false;
        }
        else if (button.gameObject.CompareTag("Hard"))
        {
            gameManager.easyMode = false;
            gameManager.mediumMode = false;
            gameManager.hardMode = true;
        }
    }
}
