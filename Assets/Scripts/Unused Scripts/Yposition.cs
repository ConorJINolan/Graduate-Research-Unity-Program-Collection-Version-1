using System.Collections;

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using UnityEngine.UI;


public class Yposition : MonoBehaviour
{

    public float YValue = 0;

    public Text YText;

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

      //  YValue = YValue;


        DisplayY(YValue);

    }

    void DisplayY(float YToDisplay)

    {

        if (YToDisplay < 0)

        {

            YToDisplay = 0;

        }

        float Yf = YToDisplay;

        YText.text = string.Format("{0:000}", Yf);

    }



}