using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// this will probably be a fully static class - Will, Allison, Dustin, Adam
public class Satellite_Spawner : MonoBehaviour
{
    // need start position in starter function

    static int name_id;     // temporary for testing
    public GameObject sat_prefab;
    public int start_satellites = 5;
    int num_sats;  //Number of satellites
    //static int MAX_SAT = 4798; //The total number of satellites in the database
    //static int start_user_satellites = 1;
    public Button btn;
    GameObject new_sat;
    static string[] data;       // Array of all of the satellites. Data for one satellite in one line, first line is junk
                                // All day is type string
    int sat_count = 1;          // An iterator for the number of satellites initialized

    // Start is called before the first frame update
    void Start()
    {
        parse_sat_dat_out();
        name_id = 1;
        btn.onClick.AddListener(generate_satellite);
    }

    void generate_satellite()
    {
        //here we get info for a satellite

        // Adam, if we can change the 2nd argument to the initial position of the satellite some particle issues will be fixed. this is low priority though
        new_sat = Instantiate(sat_prefab, new Vector3(0, 0, 0), Quaternion.identity); // create a new satellite
        name_id++;
        add_sat(new_sat);   // add initial values for the math behind orbital mechanics
    }

    void parse_sat_dat_out()    // called from start
    {
        TextAsset sat_dat_out = Resources.Load<TextAsset>("sat_dat_out");       // loading data from csv

        data = sat_dat_out.text.Split(new char[] { '\n' });            //split csv into lines called data

        for (int i = 1; i <= start_satellites; i++)   //Starts at line 1 because the first line is header
        {
            generate_satellite();
        }
    }

    // takes satellite object and gives it information from data
    void add_sat(GameObject sat_obj)
    {
        Satellite_Orbit orbit = sat_obj.GetComponent<Satellite_Orbit>();
        Satellite_Selector selector = sat_obj.GetComponent<Satellite_Selector>();
        // How to parse sat: value, data type, units (if applicaple)
        // sat[0] = Name                (String)
        // sat[1] = 1st Derivative      (Float, Motion in respect to time)
        // sat[2] = Inclination         (Float, degrees)
        // sat[3] = Right of Ascension  (Float, degrees)
        // sat[4] = Eccentricity        (Float)
        // sat[5] = Argument of Perigee (Float, degrees)
        // sat[6] = Mean Anomally       (Float, degrees)
        // sat[7] = Mean Motion         (Float, revolutions per day)
        string[] sat = data[sat_count].Split(new char[] { ',' });                   // creates array of satellite data
        sat_count += 1;

        // asign parsed data to scripts
        selector.sat_name = sat[0];         // name of satellite
        orbit.inc   = float.Parse(sat[2]);  // inclination of satellite
        orbit.omega = float.Parse(sat[3]);  // right of ascension of satellite
        orbit.e     = float.Parse(sat[4]);  // eccentricity of satellite
        orbit.w     = float.Parse(sat[5]);  // argument of perigee of satellite
        orbit.M_0   = float.Parse(sat[6]);  // mean anomaly of satellite
        orbit.n     = float.Parse(sat[7]);  // mean motion of satellite

    }
}
