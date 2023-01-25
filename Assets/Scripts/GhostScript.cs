using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    // movement
    [SerializeField]
    public float ghostSpeed;
    Vector3 axis = new Vector3(0, 1, 0);
    Vector3 forward = new Vector3(0, 0, -1); // first this direction to ensure the ghost is looking in the direction he´s moving
    Vector3 forwardX = new Vector3(1, 0, 0);
    Vector3 forwardZ = new Vector3(0, 0, 1);




    public bool isHunted;

    private Color mainColor;
    // Start is called before the first frame update
    void Start() {

        mainColor = gameObject.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update() {
        Movement();
        if (!isHunted) {
            setColor(mainColor); // nicht besonders performant
        } else {

            setColor(Color.blue);
        }


    }

    ////////////////////////////////////
    ///           Movement            //
    ////////////////////////////////////

    private void Movement() {

        float howFarToMove = ghostSpeed * Time.deltaTime;
        transform.Translate(forward * howFarToMove);
    }
    private void switchForward() {
        if (forward.x == 1) {

            forward = forwardZ;
        }
        if (forward.z == 1) {
            forward = forwardX;
        }
    }

    private void changeDirectionOfMovement(int direction) {
        switch (direction) {
            case 0:
                transform.Rotate(axis, 90);
                break;
            case 1:
                transform.Rotate(axis, -90);
                break;
            default:
                Debug.Log("unknown Direction, check call of 'ChangeDirectionOfMovement");
                break;
        }
        switchForward();
    }

    private void bounceOnWallContact() {
        //    Debug.Log("manipulate Movement");
        changeDirectionOfMovement(Random.Range(0, 1));
    }


    public void setHunted(bool h) {
        isHunted = h;
    }

    private void setColor(Color col) {
        gameObject.GetComponent<Renderer>().material.color = col;
        transform.Find("head").GetComponent<Renderer>().material.color = col;
    }

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.CompareTag("Player")) {
            //spezifizieren = player
            if (isHunted) {
                Destroy(this.gameObject);
                Debug.Log("geist gefressen");
            }
        }
        if (collision.gameObject.CompareTag("wall") ||
            collision.gameObject.CompareTag("ghost") ||
            collision.gameObject.CompareTag("Stair")) {
            bounceOnWallContact();
            //normal = hinderniss
        }
        //ignoriert = cherrys, dots
    }
}


