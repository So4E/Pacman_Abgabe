using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraController : MonoBehaviour
{   
    // Attach Coresponding Cameras
    public GameObject cameraPlayer;
    public GameObject cameraTopView;
    // So other Objects can check which playmode the game is in
    public bool in_3DView;
    // Parameters concerning the Top View ability
    public const float topViewPower_max = 1f;
    [Range(.0f,topViewPower_max)]
    public float topViewPower;
    [Range(.0f,2f)]
    public float topViewPower_refill_speed = 0f; // in percent
    [Range(.0f,2f)]
    public float topViewPower_decrease_speed = 0f; // in percent

    // Start is called before the first frame update
    void Start()
    {   
        // At the start we will have max power and be in 3D mode
        topViewPower = topViewPower_max;
        in_3DView = true;
        cameraPlayer.SetActive(true);
        cameraTopView.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {   
        // update view power
        if(!in_3DView){
            topViewPower -= topViewPower_max*topViewPower_decrease_speed*Time.deltaTime;
        }else if(topViewPower < topViewPower_max){
            topViewPower += topViewPower_max*topViewPower_refill_speed*Time.deltaTime;
        }
        // detect view mode change
        if(topViewPower > .0001f && Input.GetKey(KeyCode.Space)){
                in_3DView = false;
                cameraPlayer.SetActive(false);
                cameraTopView.SetActive(true);
        }else{
            in_3DView = true;
            cameraPlayer.SetActive(true);
            cameraTopView.SetActive(false);
        }
        

    }

    public float getPower() {
        return topViewPower;
    }
}
