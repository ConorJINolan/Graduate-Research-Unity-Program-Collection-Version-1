using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay : MonoBehaviour
{
public Transform SpawnedSphere;
public GameObject SpawnSphere;
    // Start is called before the first frame update
    void Start()
    {

        DecayOrb();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DecayOrb()
    {   

            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.StartsWith(SpawnSphere.name) && obj.name.Contains("(Clone)"))
                {
                    Destroy(obj,5);
                }
            }

    }
}
