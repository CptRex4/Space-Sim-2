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
    Vector3 deb_vel;
    public int deb_from_collision;
    //RandomGenerator num_gen;

    // New Orbital Parameters
    float e;        // Eccentricity
    float n;        // Mean motion in revolutions per day
    float M_0;      // Initial mean anomilly in degrees
    float inc;      // Inclination in degrees
    float omega;    // Right ascension of the ascending node in degrees
    float w;        // Argument of perigee in degrees

    // Start is called before the first frame update
    void Start()
    {
        sat_collider = gameObject.GetComponent<Collider>();
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

        //-------------CHANGE METHOD OF RANDOMIZATION-----------------
        Vector3 v_vec = new Vector3();                  // randomized velocity vector
        //v_vec.x = num_gen.RandomNumber(-4600, 4600);    // x randomized
        //v_vec.y = num_gen.RandomNumber(-4600, 4600);    // y randomized
        //v_vec.z = num_gen.RandomNumber(-4600, 4600);    // z randomized

        // 
        Vector3 h_vec;
        Vector3 e_vec;
        Vector3 n_vec;

        // Name                (String)
        // 1st Derivative      (Float, Motion in respect to time)
        // Inclination         (Float, degrees)
        // Right of Ascension  (Float, degrees)
        // Eccentricity        (Float)
        // Argument of Perigee (Float, degrees)
        // Mean Anomally       (Float, degrees)
        // Mean Motion         (Float, revolutions per day)

        //

        // asign parsed data to scripts
        selector.sat_name = "Debris"; /*        // name of satellite
        orbit.inc = ;  // inclination of satellite
        orbit.omega = ;  // right of ascension of satellite
        orbit.e = ;  // eccentricity of satellite
        orbit.w = ;  // argument of perigee of satellite
        orbit.M_0 = ;  // mean anomaly of satellite
        orbit.n = ;  // mean motion of satellite
        */
    }
}
