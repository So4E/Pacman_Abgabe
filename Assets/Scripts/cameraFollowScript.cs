using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowScript : MonoBehaviour
{
    //target to be followed
    public Transform target;
    // offest between camera and target
    private Vector3 offset;
    [SerializeField]
    private bool isTopViewCamera; // camera looks from the top or third person onto the target

    [SerializeField, Range(0, 1)]
    private float rotationDuration; // lerp duration 


    // Start is called before the first frame update
    void Start() {
        offset = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        //offset-Vektor mit Spielerobjekt rotieren
        //position dem neuen OffsetVektor anpassen
        Vector3 offsetRotated = target.localRotation * offset;
        if (!isTopViewCamera) {
            transform.position = offsetRotated + target.transform.position;
            //    transform.rotation = target.rotation; // geht aber nicht besonders schön anzugucken
            transform.rotation = Quaternion.Lerp(transform.localRotation, target.rotation, rotationDuration);


            //camera in richtung des objekts drehen
        } else {
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        }


    }

}
