using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToNewLevel : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            pacman_playerScript pps = other.GetComponent<pacman_playerScript>();
            gm.safeTempScore(pps.getScore(), pps.getLifes()); // safe the score to be restored 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
