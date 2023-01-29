using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotScript : MonoBehaviour
{
    private pacman_playerScript player;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<pacman_playerScript>();
        }
    }



    //add points to player
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // inform player he collects a dot
            Debug.Log("Dot added points.");
            Destroy(this.gameObject);
            player.addPoints(10);
        }
    }
}
