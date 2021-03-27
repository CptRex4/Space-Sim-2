using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Orbit : MonoBehaviour
{
    /*** Variables ***/

    // Physical Constants
    static float earth_rad = 63.71f; // R_E * 10^(-5) (unity distance)
    static float mu = 6.673f * 5.972f * Mathf.Pow(10, 24 - 11 - 5 * 3); // G * M_E * (10^(-5))^3 (unity distance)

    // Orbital Parameters
    float ra;   // Apogee radius in unity units
    float rp;   // Perigee radius in unity units
    float a;    // Semimajor axis in unity units
    float b;    // Semiminor axis in unity units
    float e;    // Eccentricity
    float n;    // Mean motion
    float E;    // True anomilly 
    float M;    // Mean anomilly
    Quaternion q_w,     // w quaternion
        q_inc,          // inc quaternion
        q_omega;        // omega quaternion
    Matrix4x4 ECI_mat;  // ECI orientation matrix

    // Calulation Variables
    int repititions = 100;  // Number of iterations to aproximate E
    public Vector3 pos;     // Position of the satelite

    // Public Orbital Parameters
    public float apogee;    // in meters
    public float perigee;   // in meters
    //public float period;
    public float inc;       // Inclination angle in degrees
    public float omega;     // Right ascension of ascending node in degrees
    public float w;         // Argument of perigree in degrees

    // Time Variables
    public GameObject clock;

    // Game_Time script
    Game_Time game_time;

    /*** Events ***/

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the initial orientation of the orbit
        update_orientation();

        // Defining the values of the orbital parameters
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
        // Update the orbit in the 2D plane
        new_orbit_2D();

        // Update the orientation of the orbit (probably inefficient; should be changed to only update when needed)
        update_orientation();

        // Reorient the plane of the orbit in 3D
        pos = ECI_mat.MultiplyVector(pos);
        transform.position = pos;
    }

    // Calculates the x and z possitions (orbit in 2D) through the eccentric_anomaly_angle E
    void new_orbit_2D() 
    {
        // Compute E
        eccentric_anomaly_angle(game_time.total_seconds);

        // Compute the 2D position of the satellite
        pos.x = a * (Mathf.Cos(E) - e);
        pos.z = b * Mathf.Sin(E);
        pos.y = 0;
    }

    // Calculate a new orientation
    void update_orientation()
    {
        // Calculate quaternions for (inc, omega, w)
        q_w = Quaternion.Euler(0f, w, 0f);          // rotate w degrees about y-axis
        q_inc = Quaternion.Euler(inc, 0f, 0f);      // rotate inc degrees about x-axis
        q_omega = Quaternion.Euler(0f, omega, 0f);  // rotate omega degrees about y-axis

        // Calculate the orientation matrix for the orbital plane
        ECI_mat = Matrix4x4.Rotate(q_omega) * Matrix4x4.Rotate(q_inc) * Matrix4x4.Rotate(q_w);
    }

    //This iterativelly calculates the True Anomaly E
    void eccentric_anomaly_angle(float time) 
    {
        float e_deg = e * Mathf.Rad2Deg;
        M = n * time; //2 * Mathf.PI * time / (period * 60.0f);
        E = M;

        for (int i = 0; i < repititions; i++)
        {
            E = E + (M + e_deg * Mathf.Sin(Mathf.Deg2Rad * E) - E) / (1 - e * Mathf.Cos(Mathf.Deg2Rad * E));
        }
    }
}
