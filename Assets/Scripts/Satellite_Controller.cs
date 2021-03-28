using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Controller : MonoBehaviour
{
    Satellite_Orbit orbit_script;
    float time_since_input;
    public float input_buffer = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        orbit_script = this.gameObject.GetComponent<Satellite_Orbit>();
        time_since_input = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time_since_input += Time.deltaTime;

        if (Input.GetKey(KeyCode.UpArrow) && this.gameObject.GetInstanceID() == Satellite_Selector.selected_sat_id && time_since_input >= input_buffer)
        {
            time_since_input = 0.0f;

            orbit_script.inc += 0.1f;
        }
        if (Input.GetKey(KeyCode.DownArrow) && this.gameObject.GetInstanceID() == Satellite_Selector.selected_sat_id && time_since_input >= input_buffer)
        {
            time_since_input = 0.0f;

            orbit_script.inc -= 0.1f;
        }
    }
}
