using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomPlayerMovementSecundary : MonoBehaviour
{

    [SerializeField]
    float speed = 2;

    float directionX;
    float directionZ;
    // Update is called once per frame
    void Update() {
        directionX = 0 * speed * Time.deltaTime;
        directionZ = 1 * speed * Time.deltaTime;
        Vector3 movement = new Vector3(directionX, 0, directionZ);
        if (Input.GetKeyUp(KeyCode.A)) {


        } else if (Input.GetKeyUp(KeyCode.D)) {

        }

    }
}
