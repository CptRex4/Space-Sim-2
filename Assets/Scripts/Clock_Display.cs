using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Clock_Display : MonoBehaviour
{
    public Text textClock;
    public GameObject clock_Display;
    Game_Time clockTime;
    //DateTime clockTime;
    //string hour;
    //string minute;
    //string seconds;
    // Start is called before the first frame update
    void Awake()
    {
        textClock = gameObject.GetComponent<Text>();
    }
    void Start()
    {
        //clockTime = clock_Display.GetComponent<Game_Time>();
        clockTime = clock_Display.GetComponent<Game_Time>();
    }

    // Update is called once per frame
    void Update()
    {
        //clockTime = DateTime.Now;
        string hour = clockTime.hours.ToString().PadLeft(2, '0'); ;
        string minute = clockTime.minutes.ToString().PadLeft(2, '0'); ;
        //string seconds = clockTime.seconds.ToString();
        textClock.text = hour + ":" + minute; //+ ":" + seconds;
    }

    /*string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }*/
}
