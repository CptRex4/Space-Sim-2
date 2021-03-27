using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Orbit : MonoBehaviour
{
    /*** Variables ***/

    // Physical Constants
    static float earth_rad = 6378.137f; // Radius of the earth in kliometers
    static float mu = 0.3986004418f; // G * M_E * (10^(-5))^3 (unity distance conversion)

    // Private Orbital Parameters
    float a;    // Semimajor axis in unity distance units
    float b;    // Semiminor axis in unity distance units
    float p;    // Semilatus rectum in unity distance units
    float ra;   // Apogee radius in unity distance units
    float rp;   // Perigee radius in unity distance units
    // Dynamic Parameters
    float r;    // Distance from origin in unity distance units
    float v;    // Speed in unity distance units per second
    float E;    // True anomilly in radians
    float M;    // Mean anomilly in radians
    Quaternion q_w,     // w quaternion
        q_inc,          // inc quaternion
        q_omega;        // omega quaternion
    Matrix4x4 ECI_mat;  // ECI orientation matrix

    // Helpful Public Variables
    public float apogee;    // Apogee altitude in kilometers
    public float perigee;   // Perigee altitude in kilometers
    public float period;    // Period in minutes
    // Dynamic Parameters
    public float altitude;  // Altitude in kilometers
    public float speed;     // Speed in kilometers/second

    // Calulation Variables
    int repititions = 100;  // Number of iterations to aproximate E
    public Vector3 pos;     // Position of the satellite
    public Vector3 vel;     // Velocity of the satellite

    // Public Orbital Parameters
    public float e;     // Eccentricity; < 0.25 for LEO
    public float M_0;   // Initial mean anomilly in degrees
    public float n;     // Mean motion in revolutions/day; > 11.25 for LEO
    public float inc;   // Inclination in degrees
    public float omega; // Right ascension of ascending node in degrees
    public float w;     // Argument of perigree in degrees

    // Time Variables
    public GameObject clock;    // Clock object
    Game_Time game_time;        // Game_Time script

    /*** Events ***/

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the initial orientation of the orbit
        update_orientation();

        // Calculate the initial axis parameters
        update_axis_variables();

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

        /*** Test Code: This is inefficient and should only be used in small-scale testing ***/
        update_orientation();
        update_axis_variables();
        update_trajectory_variables();

        // Reorient the plane of the orbit in 3D
        pos = ECI_mat.MultiplyVector(pos);
        transform.position = pos;
    }

    // Calculates the x and z coordinates (P and Q in perifocal coordinates) using E
    void new_orbit_2D() 
    {
        // Compute the True Anomilly
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

    // Calculate the axis parameters of the elipse
    void update_axis_variables()
    {
        // Calculate values of (a, b, ra, rp, p) given (n, e)
        a = Mathf.Pow(mu, 1f / 3f) / Mathf.Pow(2 * Mathf.PI * n / (24f * 3600f), 2f / 3f);    // Convert n to radians per second
        p = a * (1 - e * e);
        ra = a * (1 + e);
        rp = a * (1 - e);
        b = Mathf.Sqrt(ra * rp);

        // Calculate helpful pulbic variables
        apogee = ra * 100f - earth_rad;     // Convert from unity distance to kilometers (100)
        perigee = rp * 100f - earth_rad;
        period = (24f * 60f) / n;           // Convert from days to minutes
    }

    // Calutate the distance, altitude, and speed
    void update_trajectory_variables()
    {
        // Calculate the speed and distance in unity units
        r = p / (1 + e * Mathf.Cos(E));
        v = Mathf.Sqrt(mu * ( (2 / r) - (1 / a) ));

        // Calculate the altitude and speed in kilometers (convert from unity units)
        altitude = r * 100 - earth_rad;
        speed = v * 100;
    }

    //This iterativelly calculates the True Anomaly E
    void eccentric_anomaly_angle(float time) 
    {
        // Calculate the current meant anomilly in radians
        M = (2 * Mathf.PI * n / (24f * 3600f)) * time + Mathf.Deg2Rad * M_0;  // Convert n to radians per second and M_O to radians

        // Initialize E to M and approximate E
        E = M;

        for (int i = 0; i < repititions; i++)
        {
            E = E + (M + e * Mathf.Sin(E) - E) / (1 - e * Mathf.Cos(E));
        }
    }
}
