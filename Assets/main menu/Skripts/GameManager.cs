using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Player;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{

    public static string player_highscore = "highscore";

    public static GameManager instanceGM;
    [SerializeField]
    int mainGameScene; // you get the id of the scenes in file->buildsettings->scenes in build
    [SerializeField]
    int mainMenuScene;


    private TextMeshProUGUI highScore;
    private Button startButton, settingsButton, exitGameButton, resetScoreButton, closeSettingsButton;

    private int tempScore;
    private int tempLifes;
    private bool reenteredScene = false;


    private void Awake() {
        //    instanciateButtons();

        if (instanceGM == null) {
            instanceGM = this;
            DontDestroyOnLoad(gameObject); //
        } else {
            Destroy(gameObject);
        }

        //settingsCanvas = GameObject.FindGameObjectWithTag("SettingsCanvas");
        highScore = GameObject.FindGameObjectWithTag("HighScoreMainMenu").GetComponent<TextMeshProUGUI>();

        instanciateScoreboard();
        SceneManager.activeSceneChanged += restoreScore;


    }

    public void onClickStartGame() {
        SceneManager.LoadScene(mainGameScene);
    }


    public void onClickQuitGame() { // just works if the application was built (not if started in unity)
        Application.Quit();
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



    ////////////safe Score on reenter a new floor //////////
    public void safeTempScore(int highscore, int lifes) {
        tempScore += highscore;
        tempLifes = lifes;
        Debug.Log("score safed: " + tempScore);
    }

    public void restoreScore(Scene old, Scene current) {
        Debug.Log("new active Scene: " + current.name);
        if (current.name == "direct_gameStart") {
            Debug.Log("entered GameStartscene");
            reenteredScene = true;
        } else if (current.name == "main_menu") {
            resetData();
            reenteredScene = false;
        }

    }

    public void resetData() {
        tempScore = 0;
        tempLifes = 3;
    }

    private void Update() {
        if (reenteredScene) {
            pacman_playerScript pps = GameObject.FindGameObjectWithTag("Player").GetComponent<pacman_playerScript>();
            pps.restoreData(tempLifes, tempScore);
            Debug.Log("pacman recieved new Score: " + pps.getScore());
            Debug.Log("pacman recieved new Lifes: " + pps.getLifes());
            inGameManager inGameManager = GameObject.FindGameObjectWithTag("inGameManager").GetComponent<inGameManager>();
            inGameManager.setScoreText(pps.getScore());
            inGameManager.setLifeText(pps.getLifes());
            reenteredScene = false;
        }
    }



}
