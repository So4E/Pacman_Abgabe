using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cherrySkript : MonoBehaviour
{
    private pacman_playerScript player; 


    // Start is called before the first frame update
    void Start() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<pacman_playerScript>();
        }
    }

    // Update is called once per frame
    void Update() {
        Vector3 axis = new Vector3(0, 1, 0);
        transform.Rotate(axis, 10 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider) {
    
        if (collider.CompareTag("Player")) {
            // inform player he ate a cherry
            player.ateCherry(); 
            player.addPoints(200);
           Destroy(gameObject); 
        }
    }
}
