using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Selector : MonoBehaviour
{
    public bool is_selected;
    GameObject satellite;
    public string sat_name;
    static public bool reset_selection = false;

    // Start is called before the first frame update
    void Start()
    {
        satellite = this.gameObject;
        is_selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (reset_selection)
        {
            print(1);
        }
    }

    void OnMouseDown()
    {
        is_selected = true;
        Camera_Control.change_target(satellite.GetComponent<Satellite_Orbit>());
        //show stuff
    }
}
