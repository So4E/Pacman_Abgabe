using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowScript : MonoBehaviour
{
    public Transform target;

    private Vector3 offset;
    [SerializeField]
    private bool isTopViewCamera;

    [SerializeField, Range(0, 1)]
    private float rotationDuration;

    private float timeCount;



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
            //    transform.rotation = target.rotation; // geht aber nicht besonders sch�n anzugucken
            transform.rotation = Quaternion.Lerp(transform.localRotation, target.rotation, rotationDuration);


            //camera in richtung des objekts drehen
        } else {
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        }


    }

}
