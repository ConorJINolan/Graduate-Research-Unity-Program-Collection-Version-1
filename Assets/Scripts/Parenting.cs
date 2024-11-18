using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parenting : MonoBehaviour
{
    public Transform SpawnedSphere;
    private float a = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (a==0)
        { transform.SetParent(SpawnedSphere);
            a = 1;
        }
       
    }
}
