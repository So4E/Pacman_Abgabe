using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopViewPowerlevelBar : MonoBehaviour
{

    // attach Camera Controller so we can acces our Viewpowerlevel
    private GameObject cameraController;


    // controlling visuals
    private Slider powerLevelBar;
    private Image powerBarColor;
    private ParticleSystem particles;
    private bool in3DView;


    private void Start() {
        cameraController = GameObject.FindGameObjectWithTag("CameraController");
        powerLevelBar = gameObject.GetComponent<Slider>();
        powerBarColor = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        particles = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        in3DView = cameraController.GetComponent<CameraController>().in_3DView;
        ChangeAppearance();
    }

    void Update() {
     
        if (cameraController == null) { // somehow unity doesnt store this variable inside the cameraController so it has be to reassigned here once.
        cameraController = GameObject.FindGameObjectWithTag("CameraController");
            Debug.Log("cameraController was null");
        }
     
        // update fill level 
        powerLevelBar.value = cameraController.GetComponent<CameraController>().getPower(); // immer =0
                                                                        

        // detect mode change
        if (cameraController.GetComponent<CameraController>().in_3DView != in3DView) {
            ChangeAppearance();
        }
    }

    // change the appearance of the powerbar according to the view mode
    private void ChangeAppearance() {

        in3DView = !in3DView;

        // Apperance 3D View
        if (in3DView) {
            transform.localScale = new Vector3(2, 2, 1);
            powerBarColor.color = new Color32(19, 111, 10, 255);
            particles.Clear();
            particles.Stop();
        }
        // Apperance inTopView 
        else {
            transform.localScale = new Vector3(2, 2, 1);
            powerBarColor.color = new Color32(111, 12, 10, 255);
            particles.Play();
        }

    }


}
