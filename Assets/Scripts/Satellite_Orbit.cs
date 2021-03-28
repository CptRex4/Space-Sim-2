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

    // Control Parameters
    public Vector3 orientation; // Orientation opposite to the satelite thruster (in the ECI frame)
    public float deltaV_budget; // Total deltaV available to the player (in km/s)
    float rotation_speed;       // Speed at which you can rotate a satelite (in rad/s)
    float v_rel;                // Exhaust velocity of propelant (in unity distance per second)
    float m_dot;                // Mass exhaust rate of the propelant (in kg/s)
    float m_sat;                // Mass of the satellite without fuel (in kg)
    float m_tot;                // Total mass of the satellite and fuel remaining (in kg)

    // Calulation Variables
    int repititions = 100;  // Number of iterations to aproximate E
    public Vector3 pos;     // Position of the satellite in the ECI frame
    public Vector3 vel;     // Velocity of the satellite in the ECI frame

    // Public Orbital Parameters
    public float e;     // Eccentricity; < 0.25 for LEO
    public float M_0;   // Initial mean anomilly in degrees
    public float n;     // Mean motion in revolutions/day; > 11.25 for LEO
    public float inc;   // Inclination in degrees
    public float omega; // Right ascension of ascending node in degrees
    public float w;     // Argument of perigree in degrees

    // Time Variables
    GameObject clock;    // Clock object
    Game_Time game_time;        // Game_Time script

    // Color Variables
    Gradient final_grad;
    GradientColorKey[] final_grad_color;
    GradientAlphaKey[] final_grad_alpha;
    Gradient altitude_grad;
    GradientColorKey[] altitude_grad_color;
    GradientAlphaKey[] altitude_grad_alpha;
    ParticleSystem particles;

    //--------------------------------------------------LAST RESORT---------------------------------------------------------------------
    // ---------------------------------------------------------------------------------------------------------------------------------
    // jank position offsets
    //--LAST RESORT-- public float y_offset; // offset's the entire ellipse up or down --------------------------------------------------

    /*** Events ***/

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the initial orientation of the orbit
        update_orientation();

        // Calculate the initial axis parameters
        update_axis_variables();

        // Compute the deltaV budget
        compute_deltaV_budget();

        // Getting game_time
        // game_time.total_seconds is the total game-world seconds since the start of the program
        clock = GameObject.Find("Clock");
        game_time = clock.GetComponent<Game_Time>();

        // Create the gradient of color depending on altitude
        altitude_grad_color = new GradientColorKey[2];
        altitude_grad_color[0].color = Color.magenta;
        altitude_grad_color[0].time = 0.0f;
        altitude_grad_color[1].color = Color.cyan;
        altitude_grad_color[1].time = 1.0f;
        altitude_grad_alpha = new GradientAlphaKey[2];
        altitude_grad_alpha[0].alpha = altitude_grad_alpha[1].alpha = 1.0f;
        altitude_grad_alpha[0].time = 0.0f;
        altitude_grad_alpha[1].time = 1.0f;
        altitude_grad = new Gradient();
        altitude_grad.SetKeys(altitude_grad_color, altitude_grad_alpha);
        final_grad = new Gradient();
        final_grad_color = new GradientColorKey[2];
        final_grad_alpha = new GradientAlphaKey[2];
        final_grad_alpha[0].alpha = 1.0f;
        final_grad_alpha[0].time = 0.0f;
        final_grad_alpha[1].alpha = 0.0f;
        final_grad_alpha[1].time = 1.0f;
        particles = this.gameObject.GetComponent<ParticleSystem>();
        var color_over_time = particles.colorOverLifetime;
        color_over_time.enabled = true;

        //--------------------------------------------------LAST RESORT---------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------------------------------
        //y_offset = 0;   // the y offset is 0 until changed by the user
    }

    // Update is called once per frame
    void Update()
    {
        /*** Test Code: This is inefficient and should only be used in small-scale testing ***/
        update_orientation();
        update_axis_variables();
        update_trajectory_variables();

        // Compute the True Anomilly
        eccentric_anomaly_angle(game_time.total_seconds);

        // Compute the position of the satellite
        compute_pos_vec();

        // Compute the velocity of the satellite
        //compute_velocity_vec();

        // Update the position of the satellite object
        transform.position = pos;

        // Update the color gradient of the satellite object
        final_grad_color[0].color = final_grad_color[1].color = altitude_grad.Evaluate(altitude / 3000.0f);
        final_grad.SetKeys(final_grad_color, final_grad_alpha);
        var color_over_time = particles.colorOverLifetime;
        color_over_time.color = final_grad;
        var particles_main = particles.main;
        particles_main.startColor = altitude_grad.Evaluate(altitude / 3000.0f);
    }

    // Calculates the x and z coordinates (P and Q in perifocal coordinates) using E
    void compute_pos_vec()
    {
        // Compute the 2D position of the satellite
        pos.x = a * (Mathf.Cos(E) - e);
        pos.z = b * Mathf.Sin(E);
        pos.y = 0;

        // Rotate into the ECI frame
        pos = ECI_mat.MultiplyVector(pos);

        //--------------------------------------------------LAST RESORT---------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------------------------------
        // jank offset
        //pos.y += y_offset;  // offset the position by a y value
    }

    // Calculate the velocity from the orbital parameters and ECI matrix (in unity units)
    void compute_velocity_vec()
    {
        // Compute velocity in perifocal coordinates
        vel.x = -Mathf.Sqrt(mu / p) * Mathf.Sin(E);
        vel.z = Mathf.Sqrt(mu / p) * (e + Mathf.Cos(E));
        vel.y = 0;

        // Rotate into the ECI frame
        vel = ECI_mat.MultiplyVector(vel);
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
        apogee = ra * 100f - earth_rad;     // Convert from unity distance to kilometers (x100)
        perigee = rp * 100f - earth_rad;
        period = (24f * 60f) / n;           // Convert from days to minutes
    }

    // Calutate the distance, altitude, and speed
    void update_trajectory_variables()
    {
        // Calculate the speed and distance in unity units
        r = p / (1 + e * Mathf.Cos(E));
        v = Mathf.Sqrt(mu * ((2 / r) - (1 / a)));

        // Calculate the altitude and speed in kilometers (convert from unity units)
        altitude = r * 100 - earth_rad;
        speed = v * 100;
    }

    // Calculate the total remaining deltaV budget for the satelite
    void compute_deltaV_budget()
    {
        deltaV_budget = v_rel * Mathf.Log(m_tot / m_sat) * 100; // Convert to km/s (x100)
    }

    // compute 

    // 
    //void add_deltaV()
    //{
    //    // Compute the velocity vector
    //    compute_velocity_vec();

    //    // Compute the deltaV
    //    Vector3 deltaV = v_rel * 


    //}
}