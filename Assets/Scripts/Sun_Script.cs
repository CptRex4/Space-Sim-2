using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun_Script : MonoBehaviour
{
    public GameObject clock;

    Vector3 pos;
    Game_Time clock_script;
    float distance;
    float phi;
    int day;
    float theta;
    int days_from_perihelion;
    float declination_angle;

    // Start is called before the first frame update
    void Start()
    {
        clock_script = clock.GetComponent<Game_Time>();
        day = clock_script.days;

        if (day < 3)
        {
            days_from_perihelion = day + 365;
            if (clock_script.year % 4 == 0)
                days_from_perihelion++;
        }
        else
        {
            days_from_perihelion = day;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (day != clock_script.days)
        {
            day = clock_script.days;
            days_from_perihelion++;
            if (day == 3)
                days_from_perihelion = 0;
            theta = (2 * Mathf.PI * days_from_perihelion) / 365.256f;
            distance = (149600000.0f * (1 - Mathf.Pow(0.0167f, 2))) / (1 - 0.0167f * Mathf.Cos(theta));
            distance /= 222000f;

            declination_angle = -23.45f * Mathf.Cos(Mathf.Deg2Rad * ((360.0f/365.0f) * day + 10));
            phi = 90 + declination_angle;

            pos.x = distance * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Cos(Mathf.Deg2Rad * 180);
            pos.y = distance * Mathf.Cos(Mathf.Deg2Rad * phi);
            pos.z = distance * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Sin(Mathf.Deg2Rad * 180);

            transform.position = pos;
        }
    }
}
