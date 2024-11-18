using System.Collections;

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using UnityEngine.UI;


public class Xposition : MonoBehaviour
{

    public float XValue = 0;

    public Text XText;

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

       // XValue = XValue;


        DisplayX(XValue);

    }

    void DisplayX(float XToDisplay)

    {

        if (XToDisplay < 0)

        {

            XToDisplay = 0;

        }

        float Xf = XToDisplay;

        XText.text = string.Format("{0:000}", Xf);

    }

   

}