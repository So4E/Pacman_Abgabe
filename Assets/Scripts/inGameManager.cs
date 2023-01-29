using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class inGameManager : MonoBehaviour
{

    [SerializeField] // has to be a canvas !
    GameObject uiField; // main ui with life and score
    [SerializeField] // has to be a canvas !
    GameObject pauseMenu;
    // [SerializeField]
    TextMeshProUGUI lifeUiElement;
    //   [SerializeField]
    TextMeshProUGUI scoreUiElement;
    [SerializeField]
    GameObject gameOverScreen;
    [SerializeField]
    private TextMeshProUGUI finalScore;

    GameManager gameManager;

    private bool pause = false; // paused game or not

    private void Awake() {
        uiField.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
        lifeUiElement = GameObject.FindGameObjectWithTag("Lifes-UI").GetComponent<TextMeshProUGUI>(); // null reference if nothing found
        scoreUiElement = GameObject.FindGameObjectWithTag("Score-UI").GetComponent<TextMeshProUGUI>();


    }
    private void Update() {
        /// pause and unpause
        if (Input.GetKeyDown(KeyCode.Escape) && pause == false) {
            setPauseMenuCanvas(true);
            Time.timeScale = 0; // pause game
        } else if (Input.GetKeyDown(KeyCode.Escape) && pause == true) {
            setPauseMenuCanvas(false);
            Time.timeScale = 1f; // unpause game
        }
    }

    // called on button "resume"
    public void ReturnToGame() {
        setPauseMenuCanvas(false);
        Time.timeScale = 1f;
    }
    //called on button "restart"
    public void restartGame() {
        ReturnToGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload scene
        gameManager.resetData();
    }
    // called on button "back to main"
    public void backToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); //-> id=0 is the reference to the main menu
    }


    // b true = set  canvas on by enter the pause menu || false = return to main game

    private void setPauseMenuCanvas(bool b) {
        if (b) {
            pause = true;
            pauseMenu.SetActive(true);
        } else {
            pause = false;
            pauseMenu.SetActive(false);
        }
    }

    //set score display
    public void setScoreText(int score) {
        scoreUiElement.text = "Score " + score; // irgendwie geht das nicht...

    }
    //set Life display
    public void setLifeText(int life) {
        Debug.Log("new lifes: " + life);
        lifeUiElement.text = "Lifes " + life.ToString();
    }
    //called if paclifes in pacman PlayerScript == 0
    public void gameOver(int score) {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
        finalScore.text = score.ToString();
        Debug.Log("final Score: " + finalScore.text.ToString());
        gameManager.calculateHighScore(score);
    }
}
