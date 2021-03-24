using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Rotation : MonoBehaviour
{
    GameObject earth;   // unity object to edit rotation
    Game_Time clock_script; // object containing current time
    float theta;        // angle of the rotation of the Earth about its axis

    public GameObject clock;

    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.Find("Earth");   // find the earth object in unity
    }

    // Update is called once per frame
    void Update()
    {
        // get the script from the clock
        clock_script = clock.GetComponent<Game_Time>();

        // find the angle of the Earth in degrees from the current hour and minutes
        theta = -15 * (clock_script.hours + clock_script.minutes / 60);

        // apply the rotation to the Earth
        earth.transform.rotation = Quaternion.Euler(0, theta, 0);
    }
}
