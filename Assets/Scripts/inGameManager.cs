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


    private bool pause = false;
    private void Awake() {
        uiField.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
        lifeUiElement = GameObject.FindGameObjectWithTag("Lifes-UI").GetComponent<TextMeshProUGUI>(); // null reference if nothing found
        scoreUiElement= GameObject.FindGameObjectWithTag("Score-UI").GetComponent<TextMeshProUGUI>();


    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && pause == false) {
            setPauseMenuCanvas(true);
            Time.timeScale = 0; // pause game
        } else if (Input.GetKeyDown(KeyCode.Escape) && pause == true) {
            setPauseMenuCanvas(false);
            Time.timeScale = 1f; // unpause game
        }
        /////////////ONLY FOR DEBUG PURPOSE ////////////////////
        if (Input.GetKeyDown(KeyCode.K)) {
            Debug.Log("key K pressed");
            gameOver(20);
        }
        ////////////////////////////////////////////////////////
    }

    // called on button "resume"
    public void ReturnToGame() {
        setPauseMenuCanvas(false);
        Time.timeScale = 1f;
    }
    public void restartGame() {
        ReturnToGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.resetData();
    }
    // called on button "back to main"
    public void backToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); //-> id=0 is the reference to the main menu
    }

    /*
 * @param b true = set  canvas on by enter the pause menu || false = return to main game
 */
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

    //////Debug Purpose///////
    
    public void onClickTestButton() {
        Debug.Log("button pressed!");
    }



}
