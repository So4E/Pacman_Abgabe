using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class pacman_playerScript : MonoBehaviour
{
    public GameObject pacman;
    public float playerSpeed = 1;
    private int score;
    private int pacLifes;

    private inGameManager inGameManager;

    // Movement //////////////////////
    private float savedPlayerSpeed;
    Vector3 axis = new Vector3(0, 1, 0);
    Vector3 forward = new Vector3(1, 0, 0);
    Vector3 forwardX = new Vector3(1, 0, 0);
    Vector3 forwardZ = new Vector3(0, 0, 1);


    // Hunter/////////////////////////////
    [SerializeField]
    [Range(0f, 10f)]
    public float durationOfHunting;
    private bool hunter = false;
    private float timeToHuntLeft;
    GameObject[] ghosts;

    // invulnerable after loosing a life
    private float durationOfInvulnerable = 2;
    private bool invulnerable = false;
    private float timeToInvulnerableLeft;
    private Color mainColor;
    private Renderer rend;
    void Start() {
        savedPlayerSpeed = playerSpeed;
        inGameManager = GameObject.FindGameObjectWithTag("inGameManager").GetComponent<inGameManager>();

        inGameManager.setScoreText(score);
        inGameManager.setLifeText(pacLifes);
        rend = GetComponent<Renderer>();
        mainColor = rend.material.color;
    }
    //calculate how far to move in a frame
    void MoveAlwaysForward() {
        if (playerSpeed != 0) {
            float howFarToMove = playerSpeed * Time.deltaTime;
            transform.Translate(forward * howFarToMove);

        }
    }
    // add points to the score
    public void addPoints(int howMany) {
        score += howMany;
        inGameManager.setScoreText(score);
    }
    //on switch direction set new forward to avoid endless numbers on transform.rotation.y
    private void switchForward() {
        if (forward.x == 1) {
            forward = forwardZ;
        }
        if (forward.z == 1) {
            forward = forwardX;
        }
    }



    void Update() {
        //------------------------player movement
        //turn right
        bool onKeyPressedD = Input.GetKeyDown(KeyCode.D);
        bool onKeyPressedArrowRight = Input.GetKeyDown(KeyCode.RightArrow);

        if (onKeyPressedD || onKeyPressedArrowRight) {
            transform.Rotate(axis, 90);
            switchForward();

            if (playerSpeed == 0) {
                playerSpeed = savedPlayerSpeed;
            }
        }
        //turn left
        bool onKeyPressedA = Input.GetKeyDown(KeyCode.A);
        bool onKeyPressedArrowLeft = Input.GetKeyDown(KeyCode.LeftArrow);
        //---------------------------//
        if (onKeyPressedA || onKeyPressedArrowLeft) {
            transform.Rotate(axis, -90);
            switchForward();
            if (playerSpeed == 0) {
                playerSpeed = savedPlayerSpeed;
            }
        }


        //always move forward at player speed
        MoveAlwaysForward();
        trackHunting(); // keeps track if hes hunter or not
        isInvulnerableAfterHit();
    }

    // after eating a cherry pacman becomes a hunter for a view seconds.
    public void ateCherry() {
        Debug.Log("ate Cherry");
        hunter = true;
        Debug.Log("hunter: " + hunter);
        timeToHuntLeft = durationOfHunting;
        setGhostToHunted(true);
        rend.material.color = Color.blue;
    }
    // logic for the hunting process -> if hes hunter or not
    private void trackHunting() {
        if (hunter) { // performanter
            // keep track of Hunting/last ate cherry
            if (timeToHuntLeft > 0) {
                timeToHuntLeft -= Time.deltaTime;
            } else {
                hunter = false;
                rend.material.color = mainColor;
                setGhostToHunted(false);
                Debug.Log("hunter: " + hunter);
            }
        }
    }
    // inform ghosts they are hunted
    private void setGhostToHunted(bool state) {
        ghosts = GameObject.FindGameObjectsWithTag("ghost");
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].GetComponent<GhostScript>().setHunted(state);
        }
    }
    // make sure, pacman has a chance to escape after hitting a ghost.
    private void isInvulnerableAfterHit() {
        if (invulnerable) {
            if (timeToInvulnerableLeft > 0) {
                timeToInvulnerableLeft -= Time.deltaTime;
            } else {
                invulnerable = false;
            }
        }
    }

    // stop moving on wallcontact
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("wall")) {
            playerSpeed = 0;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("ghost") && !hunter && !invulnerable) { // loosing a life on Ghostcontact
            invulnerable = true;
            timeToInvulnerableLeft = durationOfInvulnerable;
            pacLifes--;
            inGameManager.setLifeText(pacLifes);
            if (pacLifes == 0) {
                inGameManager.gameOver(score);
            }
        } else if (other.gameObject.CompareTag("ghost") && hunter) { // eating a ghost if pacman is the hunter
            addPoints(100);


        }
    }

    public int getLifes() {
        return pacLifes;
    }
    public int getScore() {
        return score;
    }
    // restore data after reloading the game-Scene
    public void restoreData(int lifes, int score) {
        pacLifes = lifes;
        this.score = score;
    }
}
