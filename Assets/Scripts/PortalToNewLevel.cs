using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToNewLevel : MonoBehaviour
{

    void Update() {
        Vector3 axis = new Vector3(0, 1, 0); // rotate the portal around itself
        transform.Rotate(axis, 20 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); // find  gamemanager ->
                                                                                                          // the only object, which is not destroyed on reloading 
                                                                                                          // a scene
            pacman_playerScript pps = other.GetComponent<pacman_playerScript>(); // find player 
            gm.safeTempScore(pps.getScore(), pps.getLifes()); // safe the score to be restored from player into the gamemanager
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload the scene
        }
    }
}
