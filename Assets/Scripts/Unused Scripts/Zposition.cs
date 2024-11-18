using System.Collections;

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using UnityEngine.UI;


public class Zposition : MonoBehaviour
{

    public float ZValue = 0;

    public Text ZText;

    // Start is called before the first frame update
    void Start()
    {
        //XValueCurrent = Xvalue;
    }

    // Update is called once per frame
    void Update()
    {
        // Temp Work Around


        //XValue = XValueCurrent;

       // ZValue = ZValue;


        DisplayZ(ZValue);

    }

    void DisplayZ(float ZToDisplay)

    {

        if (ZToDisplay < 0)

        {

            ZToDisplay = 0;

        }

        float Zf = ZToDisplay;

        ZText.text = string.Format("{0:000}", Zf);

    }

   

}