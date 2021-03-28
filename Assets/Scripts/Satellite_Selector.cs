using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Selector : MonoBehaviour
{
    public static int selected_sat_id;
    GameObject satellite;
    public string sat_name;

    // Start is called before the first frame update
    void Start()
    {
        satellite = this.gameObject;
        selected_sat_id = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // if rmb pressed stop displays
    }

    void OnMouseDown()
    {
        selected_sat_id = satellite.GetInstanceID();
        Camera_Control.change_target(satellite.GetComponent<Satellite_Orbit>());
        //show stuff
    }
}
