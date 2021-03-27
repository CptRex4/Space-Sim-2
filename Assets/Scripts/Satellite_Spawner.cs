using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this will probably be a fully static class - Will
public class Satellite_Spawner : MonoBehaviour
{
    static int name_id;     // temporary for testing
    public GameObject sat_prefab;

    // Start is called before the first frame update
    void Start()
    {
        name_id = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (name_id == 1)   // TEMPORARY FOR TESTING
        {
            print("new");
            // the following code should only run if new satellite button is pressed

            // take a line of input from the .txt and separate it into important variables

            Instantiate(sat_prefab, new Vector3(0, 0, 0), Quaternion.identity); // create a new satellite
            name_id++;

            // give correct data for orbital mechanics to satellite
           
        }
    }
}
