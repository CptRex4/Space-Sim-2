using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Rotation : MonoBehaviour
{
    

    GameObject earth;
    Game_Time clock_script;
    int theta;

    public GameObject clock;

    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.Find("Earth");
    }

    // Update is called once per frame
    void Update()
    {
        clock_script = clock.GetComponent<Game_Time>();

        theta = (int)(15 * (clock_script.hours + clock_script.minutes / 60));

        earth.transform.rotation = Quaternion.Euler(0, theta, 0);

    }
}
