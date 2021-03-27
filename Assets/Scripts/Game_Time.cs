using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Time : MonoBehaviour
{
    /*** time variables ***/

    // The total game-world seconds since the start of the program
    public float total_seconds;
    
    // Game world clock (modular)
    public float minutes;
    public int hours;
    public int days;
    public int year;

    // real time since last in-game hour change
    float game_time;    

    // Rate of game-world time passage per player time (min/s)
    public float time_rate;

    /*** Events ***/

    // Start is called before the first frame update
    void Start()
    {
        // initial values of time variables
        total_seconds = 0.0f;
        minutes = 0.0f;
        hours = 0;
        days = 1;
        year = 2021;

        game_time = 0.0f;

        time_rate = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Change in game-world time (in minutes) since last frame based on time_rate.
        float time_change = time_rate * Time.deltaTime;

        game_time += time_change / 60.0f;       // increment game_time by the number of hours passed in the game-world
        minutes += time_change;                 // increment the minutes from the game-world's clock
        total_seconds += time_change * 60.0f;   // increment the total seconds passed in the game-world


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
