using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CollisionCounter : MonoBehaviour
{
    public Text text;
    public int num;

    // Start is called before the first frame update
    void Start()
    {
        
      
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = num.ToString();


    }
}
