using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{

    public static string player_highscore = "highscore"; // id to write the highscore to file -> enables to restore data after game is closed

    public static GameManager instanceGM; // Implementation of a so called Singleton -> just one object of this can exist
    [SerializeField]
    int mainGameScene; // you get the id of the scenes in file->buildsettings->scenes in build
    [SerializeField]
    int mainMenuScene;

    // safe score on reloading the scene
    private int tempScore;
    private int tempLifes;
    private bool reenteredScene = false;


    private void Awake() {
        //    instanciateButtons();

        if (instanceGM == null) {
            instanceGM = this;
            DontDestroyOnLoad(gameObject); // make sure GameManager is not destroyed on load a new scene
        } else {
            Destroy(gameObject);
        }

        instanciateScoreboard();
        SceneManager.activeSceneChanged += restoreScore;


    }
    // called on button "startGame"
    public void onClickStartGame() {
        SceneManager.LoadScene(mainGameScene);
    }

    //called on button "quitGame"
    public void onClickQuitGame() { // just works if the application was built (not if started in unity)
        Application.Quit();
    }
    //called on button "resetHighscore"
    public void onClickResetHighscore() {
        safeScoreboard(0);
    }
    //called on button "MainMenu"
    public void onClickLoadMainMenu() {
        SceneManager.LoadScene(mainMenuScene);
    }


    //////////////////////////////
    //         Scoreboard    /////
    //////////////////////////////
    public void instanciateScoreboard() {
        updateScoreboard(PlayerPrefs.GetInt(player_highscore)); // first print at the start of the scene
    }
    public void calculateHighScore(int lastScore) { // update the score, if a new highscore is set
        if (lastScore >= PlayerPrefs.GetInt(player_highscore)) {
            safeScoreboard(lastScore);

        }
    }
    // update the scoreboard if theres a new Highscore
    public void updateScoreboard(int score) {
        GameObject.FindGameObjectWithTag("HighScoreMainMenu").GetComponent<TextMeshProUGUI>().text = score.ToString(); // storing in a variable doesnt work somehow
    }
    // safe a new Highscore into the PlayerPrefs, a small database -> so the score is reloaded if you restart the game
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
    /// <summary>
    /// this Method is called whenever a new scene is loaded.
    /// ensure that the playerData is only safed if needed
    /// </summary>
    /// <param name="old"></param> unused -> see docu of "activeSceneChanged"
    /// <param name="current"></param> the name of the scene which is loaded right now
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
    // reset data if not in game
    public void resetData() {
        tempScore = 0;
        tempLifes = 3;
    }

    private void Update() {
        if (reenteredScene) {
            pacman_playerScript pps = GameObject.FindGameObjectWithTag("Player").GetComponent<pacman_playerScript>();
            pps.restoreData(tempLifes, tempScore); // restore data if in game
            Debug.Log("pacman recieved new Score: " + pps.getScore());
            Debug.Log("pacman recieved new Lifes: " + pps.getLifes());
            inGameManager inGameManager = GameObject.FindGameObjectWithTag("inGameManager").GetComponent<inGameManager>();
            inGameManager.setScoreText(pps.getScore()); // set score and life right at the start of the game, not first after collecting something
            inGameManager.setLifeText(pps.getLifes());
            reenteredScene = false;
        }
    }



}
