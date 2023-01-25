using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class pacman_playerScript : MonoBehaviour
{
    public GameObject pacman;
    public int score;
    public float playerSpeed = 1;
    public int pacLifes;

    private inGameManager inGameManager;

    // Movement //////////////////////
    private float savedPlayerSpeed;
    Vector3 axis = new Vector3(0, 1, 0);
    Vector3 forward = new Vector3(1, 0, 0);
    Vector3 forwardX = new Vector3(1, 0, 0);
    Vector3 forwardZ = new Vector3(0, 0, 1);
    Rigidbody rig;

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
    void Start() {
        rig = GetComponent<Rigidbody>();

        savedPlayerSpeed = playerSpeed;
        score = 0;
        pacLifes = 3;
        inGameManager = GameObject.FindGameObjectWithTag("inGameManager").GetComponent<inGameManager>();

        inGameManager.setScoreText(score);
        inGameManager.setLifeText(pacLifes);
        mainColor = gameObject.GetComponent<Renderer>().material.color;
    }

    void MoveAlwaysForward() {
        if (playerSpeed != 0) {
            float howFarToMove = playerSpeed * Time.deltaTime;
            transform.Translate(forward * howFarToMove);
            // Debug.Log("forward Vector: " + forward);
            //   rig.AddForce(forward * howFarToMove, ForceMode.Force);
        }
    }

    public void addPoints(int howMany) {
        score += howMany;
        inGameManager.setScoreText(score);
    }

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
        //fix/round angle to 90� bases, use LerpAngel or Mathf.Round <-(CAREFUL!)
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


    public void ateCherry() {
        Debug.Log("ate Cherry");
        hunter = true;
        Debug.Log("hunter: " + hunter);
        timeToHuntLeft = durationOfHunting;
        setGhostToHunted(true);
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    private void trackHunting() {
        if (hunter) { // performanter
            // keep track of Hunting/last ate cherry
            if (timeToHuntLeft > 0) {
                timeToHuntLeft -= Time.deltaTime;
            } else {
                hunter = false;
                gameObject.GetComponent<Renderer>().material.color = mainColor;
                setGhostToHunted(false);
                Debug.Log("hunter: " + hunter);
            }
        }
    }

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


    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("wall")) {
            playerSpeed = 0;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("ghost") && !hunter && !invulnerable) {
            invulnerable = true;
            timeToInvulnerableLeft = durationOfInvulnerable;
            pacLifes--;
            inGameManager.setLifeText(pacLifes);
            if (pacLifes == 0) {
                inGameManager.gameOver(score);
            }
        } else if (other.gameObject.CompareTag("ghost") && hunter) {
            addPoints(100);


        }
    }
}
