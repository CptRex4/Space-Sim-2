using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Debris_Spawner : MonoBehaviour
{
    Collider sat_collider;
    public GameObject debris_prefab;
    GameObject new_deb;
    Vector3 deb_pos;
    public int deb_from_collision;

    // Time Variables
    GameObject clock;       // Clock object
    Game_Time game_time;    // Game_Time script

    // Start is called before the first frame update
    void Start()
    {
        sat_collider = gameObject.GetComponent<Collider>();

        // Getting game_time
        // game_time.total_seconds is the total game-world seconds since the start of the program
        clock = GameObject.Find("Clock");
        game_time = clock.GetComponent<Game_Time>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sat_collider.isTrigger)
        {
            for (int i = 0; 1 < deb_from_collision; i++)
            {
                new_debris();
            }
            // -------------------------------DELETE SATELLITES------------------------------------
        }
    }

    void new_debris()
    {
        new_deb = Instantiate(debris_prefab, new Vector3(0, 0, 0), Quaternion.identity);
        add_debris(new_deb);
    }

    void add_debris(GameObject deb_obj)
    {
        Satellite_Orbit orbit = deb_obj.GetComponent<Satellite_Orbit>();            // get the orbit script of the debris
        Satellite_Selector selector = deb_obj.GetComponent<Satellite_Selector>();   // get the selector script of the debris
        deb_pos = deb_obj.transform.position;                                       // get the position (xyz) of the debris

        // Random Velocity Vecotor in unity distance units per second
        Vector3 deb_vel = UnityEngine.Random.Range(0f, 7823.2f) / 100000f * UnityEngine.Random.insideUnitSphere;    // Convert from meters

        // Compute the three fundamental vectors
        Vector3 h_vec = Vector3.Cross(deb_vel, deb_pos);    // Specific angular momentum (unity distance units); Flip cross-product for left-handed coordinate system
        Vector3 e_vec = ( (deb_vel.sqrMagnitude - Satellite_Orbit.mu / deb_pos.magnitude) * deb_pos - Vector3.Dot(deb_pos, deb_vel) * deb_vel) / Satellite_Orbit.mu;    // Eccentricity vector
        Vector3 n_vec = Vector3.Cross(h_vec, Vector3.up);   // Node vector (unity distance units); Flip cross-product for left-handed coordinate system

        // Name                (String)
        // 1st Derivative      (Float, Motion in respect to time)
        // Inclination         (Float, degrees)
        // Right of Ascension  (Float, degrees)
        // Eccentricity        (Float)
        // Argument of Perigee (Float, degrees)
        // Mean Anomally       (Float, degrees)
        // Mean Motion         (Float, revolutions per day)

        // Assign orbital parameters to the script
        selector.sat_name = "Debris";   // Name of the satellite
        orbit.e = e_vec.magnitude;      // Eccentricity
        orbit.inc = Mathf.Acos(h_vec.y / h_vec.magnitude);      // Inclination in degrees
        orbit.omega = Mathf.Acos(n_vec.x / n_vec.magnitude) * (n_vec.z > 0 ? 1f : -1f);     // Right ascension of the ascending node in degrees
        orbit.w = Mathf.Acos(Vector3.Dot(n_vec, e_vec) / (n_vec.magnitude * e_vec.magnitude)) * (e_vec.y > 0 ? 1f : -1f);   // Argument of perigee in degrees

        // Extra calculations
        float p = h_vec.sqrMagnitude / Satellite_Orbit.mu;  // Semilatus rectum in unity distance units
        float a = p / (1f - orbit.e * orbit.e);             // Semimajor axis in unity distance units

        // Calculate mean motion in revolutions per day
        orbit.n = Mathf.Sqrt(Satellite_Orbit.mu / Mathf.Pow(a, 3)) * (24f * 3600f) / (2f * Mathf.PI);   // Convert to revolutions per day

        // True anomaly at epoch in radians
        float E_0 = Mathf.Acos(Vector3.Dot(deb_pos, e_vec) / (deb_pos.magnitude * e_vec.magnitude)) * (Vector3.Dot(deb_pos, deb_vel) > 0 ? 1f : -1f);

        // Calculate the mean anomaly at epoch in degrees
        orbit.M_0 = Mathf.Rad2Deg * (E_0 - orbit.e * Mathf.Sin(E_0)) - 360f * orbit.n * game_time.total_seconds / (24f * 3600f);
    }
}
