using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Time : MonoBehaviour
{
    public float minutes;
    public int hours;
    public int days;
    public int year;
    float game_time;

    // Start is called before the first frame update
    void Start()
    {
        minutes = 0.0f;
        hours = 0;
        days = 1;
        year = 2021;
        game_time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        game_time += Time.deltaTime;
        minutes += Time.deltaTime * 60;
        if (game_time >= 1.0f)
        {
            minutes = 0;
            hours++;
            game_time = 0.0f;

            if (hours >= 24)
            {
                hours -= 24;
                days++;
            }
            if ((days > 365 && year % 4 != 0))
            {
                year++;
                days -= 365;
            }
            if (days > 366)
            {
                year++;
                days -= 366;
            }
        }
    }
}
