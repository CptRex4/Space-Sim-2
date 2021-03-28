using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Selector : MonoBehaviour
{
<<<<<<< HEAD
    public bool is_selected;
    GameObject satellite;
    public string sat_name;
    static public bool reset_selection = false;
=======
    public static int selected_sat_id;
    GameObject satellite;
    public string sat_name;
>>>>>>> 7ef7ba8bea2bac49ad101eb96f43558e27cb533d

    // Start is called before the first frame update
    void Start()
    {
        satellite = this.gameObject;
<<<<<<< HEAD
        is_selected = false;
=======
        selected_sat_id = 0;
>>>>>>> 7ef7ba8bea2bac49ad101eb96f43558e27cb533d
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        if (reset_selection)
        {
            print(1);
        }
=======
        // if rmb pressed stop displays
>>>>>>> 7ef7ba8bea2bac49ad101eb96f43558e27cb533d
    }

    void OnMouseDown()
    {
<<<<<<< HEAD
        is_selected = true;
=======
        selected_sat_id = satellite.GetInstanceID();
>>>>>>> 7ef7ba8bea2bac49ad101eb96f43558e27cb533d
        Camera_Control.change_target(satellite.GetComponent<Satellite_Orbit>());
        //show stuff
    }
}
