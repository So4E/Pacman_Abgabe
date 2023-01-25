using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    float lerpInterpolation = 0;
    float lerpMaxInterpolation = 1;
    // Update is called once per frame
    void Update() {

        transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Lerp(0, 180, lerpInterpolation), 0));

        lerpInterpolation += (float)0.5 * Time.deltaTime;
        if (lerpInterpolation == lerpMaxInterpolation) { 
            lerpInterpolation = 0; }

    }


}
