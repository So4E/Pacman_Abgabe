using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotsSpawnerSimple : MonoBehaviour
{
    public Transform Dot;
    // Start is called before the first frame update
    void Start()
    {
        int numberOfObjects = 0;
        string nameOfObject = "DotClone";
        while (numberOfObjects < 4)
        {
            var position = new Vector3(Random.Range(5, -5), 0.02f, Random.Range(5, -5));
            Transform dot = Instantiate(Dot, position, Quaternion.identity);
            dot.name = nameOfObject + numberOfObjects;
            numberOfObjects++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
