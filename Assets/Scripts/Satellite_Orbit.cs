using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Orbit : MonoBehaviour
{
    float earth_rad;
    static float mu = 6.673f * 5.972f * Mathf.Pow(10, 13);
    float ra;
    float rp;
    float a;
    float b;
    float e;
    float n;
    float E;
    float M;
    int repititions = 100;
    Vector3 pos;

    public float apogee;
    public float perigee;
    public float period;
    public float time;

    public GameObject clock;
    Game_Time game_time;

    // Start is called before the first frame update
    void Start()
    {
        // Defining the values of the constants
        earth_rad = 63.71f;
        ra = earth_rad + apogee / 100000.0f;
        rp = earth_rad + perigee / 100000.0f;
        a = (ra + rp) / 2;
        b = Mathf.Sqrt(ra * rp);
        e = ra / a - 1;
        n = Mathf.Sqrt(mu / Mathf.Pow(a, 3));

        // Getting game_time
        // game_time.total_seconds is the total game-world seconds since the start of the program
        clock = GameObject.Find("Clock");
        game_time = clock.GetComponent<Game_Time>();

    }

    // Update is called once per frame
    void Update()
    {
        new_orbit_2D();
    }

    // Calculates the x and z possitions (orbit in 2D) through the eccentric_anomaly_angle E
    void new_orbit_2D() 
    {
        eccentric_anomaly_angle(game_time.total_seconds);
        pos.x = a * (Mathf.Cos(E) - e);
        pos.z = b * Mathf.Sin(E);
        transform.position = pos;
    }

    //This iterativelly calculates the True Anomaly E
    void eccentric_anomaly_angle(float time) 
    {
        float e_deg = e * Mathf.Rad2Deg;
        M = 2 * Mathf.PI * time / (period * 60.0f);
        E = M;

        for (int i = 0; i < repititions; i++)
        {
            E = E + (M + e_deg * Mathf.Sin(Mathf.Deg2Rad * E) - E) / (1 - e * Mathf.Cos(Mathf.Deg2Rad * E));
        }
    }
}
