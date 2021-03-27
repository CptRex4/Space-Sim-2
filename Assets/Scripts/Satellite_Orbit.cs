using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Orbit : MonoBehaviour
{
    /*** Variables ***/

    // Physical Constants
    //static float earth_rad = 63.71f; // R_E * 10^(-5) (unity distance)
    static float mu = 6.673f * 5.972f * Mathf.Pow(10, 24 - 11 - 5 * 3); // G * M_E * (10^(-5))^3 (unity distance)

    // Private Orbital Parameters
    float a;    // Semimajor axis in unity distance units
    float b;    // Semiminor axis in unity distance units
    float E;    // True anomilly in radians
    float M;    // Mean anomilly in radians
    Quaternion q_w,     // w quaternion
        q_inc,          // inc quaternion
        q_omega;        // omega quaternion
    Matrix4x4 ECI_mat;  // ECI orientation matrix

    // Helpful Public Variables
    public float ra;   // Apogee radius in meters
    public float rp;   // Perigee radius in meters

    // Calulation Variables
    int repititions = 100;  // Number of iterations to aproximate E
    public Vector3 pos;     // Position of the satellite
    public Vector3 vel;     // Velocity of the satellite

    // Public Orbital Parameters
    //public float apogee;
    //public float perigee;
    //public float period;
    public float e;     // Eccentricity; < 0.25 for LEO
    public float M_0;   // Initial mean anomilly in degrees
    public float n;     // Mean motion in revolutions/day; > 11.25 for LEO
    public float inc;   // Inclination in degrees
    public float omega; // Right ascension of ascending node in degrees
    public float w;     // Argument of perigree in degrees

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

        // Calculate the initial axis parameters
        update_axes();

        // Defining the values of the orbital parameters
        //ra = earth_rad + apogee / 100000.0f;
        //rp = earth_rad + perigee / 100000.0f;
        //a = (ra + rp) / 2;
        //b = Mathf.Sqrt(ra * rp);
        //e = ra / a - 1;
        //n = Mathf.Sqrt(mu / Mathf.Pow(a, 3));

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
    void update_axes()
    {
        // Calculate values of (a, b, ra, rp) given (n, e)
        a = Mathf.Pow(mu, 1f / 3f) / Mathf.Pow(2 * Mathf.PI * n / (24f * 3600f), 2f / 3f);    // Convert n to radians per second
        ra = a * (1 + e);
        rp = a * (1 - e);
        b = Mathf.Sqrt(ra * rp);

        // Convert (ra, rp) to meters
        ra *= 100000f;
        rp *= 100000f;
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
