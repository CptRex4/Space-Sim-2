using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// this will probably be a fully static class - Will and Allison
public class Satellite_Spawner : MonoBehaviour
{
    // need start position in starter function

    static int name_id;     // temporary for testing
    public GameObject sat_prefab;
    //static int start_satellites = 10;
    //static int start_user_satellites = 1;
    public Button btn;

    // Start is called before the first frame update
    void Start()
    {
        name_id = 1;
        btn.onClick.AddListener(generate_satellite);
    }

    void Testing()
    {
        print(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (name_id == 1)   // TEMPORARY FOR TESTING
        {
            generate_satellite();
           
        }
    }


    void generate_satellite()
    {
        //here we get info for a satellite

        // Adam, if we can change the 2nd argument to the initial position of the satellite some particle issues will be fixed. this is low priority though
        Instantiate(sat_prefab, new Vector3(0, 0, 0), Quaternion.identity); // create a new satellite
        name_id++;
    }

    // read file and process it here and return variables to generate satellite

    // somewhere (start?) loop generate_satellite several times over
}
