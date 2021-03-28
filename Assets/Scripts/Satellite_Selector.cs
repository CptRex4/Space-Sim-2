using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Selector : MonoBehaviour
{
<<<<<<< HEAD
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
=======
    public static int selected_sat_id = 0;
    GameObject satellite;
    public string sat_name;
    float time_since_creation;
>>>>>>> 9ddfd7bdb1967a0477addb15a06f00bb5cd62aae

    // Start is called before the first frame update
    void Start()
    {
        satellite = this.gameObject;
<<<<<<< HEAD
<<<<<<< HEAD
        is_selected = false;
=======
        selected_sat_id = 0;
>>>>>>> 7ef7ba8bea2bac49ad101eb96f43558e27cb533d
=======
        time_since_creation = 0;
        ParticleSystem particles = this.gameObject.GetComponent<ParticleSystem>();
        var emissions = particles.emission;
        emissions.enabled = false;
>>>>>>> 9ddfd7bdb1967a0477addb15a06f00bb5cd62aae
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        if (reset_selection)
        {
            print(1);
        }
=======
        // if rmb pressed stop displays
>>>>>>> 7ef7ba8bea2bac49ad101eb96f43558e27cb533d
=======
        time_since_creation += Time.deltaTime;
        if (time_since_creation >= 0.1f)
        {
            ParticleSystem particles = this.gameObject.GetComponent<ParticleSystem>();
            var emissions = particles.emission;
            emissions.enabled = true;
        }
>>>>>>> 9ddfd7bdb1967a0477addb15a06f00bb5cd62aae
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
