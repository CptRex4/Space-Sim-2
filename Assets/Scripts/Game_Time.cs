using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Time : MonoBehaviour
{
    // time variables
    public float minutes;
    public int hours;
    public int days;
    public int year;

    float game_time;    // real time since last in-game hour change

    // Start is called before the first frame update
    void Start()
    {
        // initial values of time variables
        minutes = 0.0f;
        hours = 0;
        days = 1;
        year = 2021;

        game_time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        game_time += Time.deltaTime;    // increase game_time by the time since last frame
        minutes += Time.deltaTime * 60; // since 1s is 1 hour, you can multiply a fraction of time by 60 to get minutes
        
        if (game_time >= 1.0f)  // runs once per second
        {
            minutes = 0;    // reset minutes
            hours++;        // increment hours
            game_time -= 1; // decrement game_time

            if (hours >= 24)    // if 24+ hours, increase days and reset hours
            {
                hours -= 24;
                days++;
            }

            // if over day 365 and not leap year, increase year and reset days
            if ((days > 365 && year % 4 != 0))
            {
                year++;
                days -= 365;
            }

            // if over day 366 increase year and reset days
            if (days > 366)
            {
                year++;
                days -= 366;
            }
        }
    }
}
