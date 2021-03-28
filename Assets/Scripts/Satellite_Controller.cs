using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Controller : MonoBehaviour
{
    Satellite_Orbit orbit_script;

    // Start is called before the first frame update
    void Start()
    {
        orbit_script = this.gameObject.GetComponent<Satellite_Orbit>();
    }

    // Update is called once per frame
    void Update()
    {
        print(this.gameObject.GetInstanceID());
        print(Satellite_Selector.selected_sat_id);
        if (Input.GetKey(KeyCode.UpArrow) && this.gameObject.GetInstanceID() == Satellite_Selector.selected_sat_id)
        {
            orbit_script.inc += 0.009f;
        }
        if (Input.GetKey(KeyCode.DownArrow) && this.gameObject.GetInstanceID() == Satellite_Selector.selected_sat_id)
        {
            orbit_script.inc -= 0.009f;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && this.gameObject.GetInstanceID() == Satellite_Selector.selected_sat_id)
        {
            orbit_script.omega += 0.1f;
        }
        if (Input.GetKey(KeyCode.RightArrow) && this.gameObject.GetInstanceID() == Satellite_Selector.selected_sat_id)
        {
            orbit_script.omega -= 0.1f;
        }
        if (orbit_script.omega < -10.0f)
        {
            orbit_script.omega = -10.0f;
        }
        else if (orbit_script.omega > 10.0f)
        {
            orbit_script.omega = 10.0f;
        }
    }
}
