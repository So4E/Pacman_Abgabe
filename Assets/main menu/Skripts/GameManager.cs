using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Player;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    public static string player_highscore = "highscore";

    public static GameManager instanceGM;
    [SerializeField]
    int mainGameScene; // you get the id of the scenes in file->buildsettings->scenes in build
    [SerializeField]
    int mainMenuScene;

    [SerializeField]
    private GameObject settingsCanvas; // canvas

    private TextMeshProUGUI highScore;
    private Button startButton, settingsButton, exitGameButton, resetScoreButton, closeSettingsButton;



    private void Awake() {
        //    instanciateButtons();

        if (instanceGM == null) {
            instanceGM = this;
            DontDestroyOnLoad(gameObject); // l�d das gameObject mit in die n�chste Scene anstatt es zu zerst�ren, Object das this skript h�lt
        } else {
            Destroy(gameObject);
        }

        //settingsCanvas = GameObject.FindGameObjectWithTag("SettingsCanvas");
        highScore = GameObject.FindGameObjectWithTag("HighScoreMainMenu").GetComponent<TextMeshProUGUI>();


        instanciateScoreboard();
        settingsCanvas.SetActive(false);

    }


    public void onClickStartGame() {
        SceneManager.LoadScene(mainGameScene);
    }

    public void onClickSettings() { // unused
        Debug.Log("onClicked Settings called");
        if (settingsCanvas == null) {
            Debug.Log("settings is null!"); // look at pausemenu in inGameManager -> same logic but working 
        }
        settingsCanvas.SetActive(true);      // settings are null, but why

    }
    public void onClickQuitGame() { // just works if the application was built (not if started in unity)
        Application.Quit();
    }
    public void onClickCloseSettings() { // unused 
        settingsCanvas.SetActive(false);
    }

    public void onClickResetHighscore() {
        safeScoreboard(0);
    }

    public void onClickLoadMainMenu() {
        SceneManager.LoadScene(mainMenuScene);
    }


    //////////////////////////////
    //         Scoreboard    /////
    //////////////////////////////
    public void instanciateScoreboard() {
        updateScoreboard(PlayerPrefs.GetInt(player_highscore));
    }
    public void calculateHighScore(int lastScore) {
        if (lastScore >= PlayerPrefs.GetInt(player_highscore)) {
            safeScoreboard(lastScore);

        }
    }
    public void updateScoreboard(int score) {
        GameObject.FindGameObjectWithTag("HighScoreMainMenu").GetComponent<TextMeshProUGUI>().text = score.ToString(); // storing in a variable doesnt work somehow
    }

    private void safeScoreboard(int highscore) {
        PlayerPrefs.SetInt(player_highscore, highscore);
        Debug.Log("new highscore safed! " + highscore);
        updateScoreboard(highscore);
    }

    // unused
    private void instanciateButtons() {
        Debug.Log("instanciateButtons called");
        startButton = GameObject.FindGameObjectWithTag("StartGameButton").GetComponent<Button>();

        startButton.onClick.AddListener(onClickStartGame);

        settingsButton = GameObject.FindGameObjectWithTag("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.AddListener(onClickSettings);

        exitGameButton = GameObject.FindGameObjectWithTag("ExitGameButton").GetComponent<Button>();
        exitGameButton.onClick.AddListener(onClickQuitGame);

        resetScoreButton = GameObject.FindGameObjectWithTag("ResetScoreButton").GetComponent<Button>();
        resetScoreButton.onClick.AddListener(onClickResetHighscore);

        closeSettingsButton = GameObject.FindGameObjectWithTag("closeSettingsButton").GetComponent<Button>();
        closeSettingsButton.onClick.AddListener(onClickCloseSettings);
    }

}
