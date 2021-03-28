using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Selector : MonoBehaviour
{

    public bool is_selected;
    GameObject satellite;
    public string sat_name;
    static public bool reset_selection = false;
    public static int selected_sat_id = 0;
    float time_since_creation;

    // Start is called before the first frame update
    void Start()
    {
        satellite = this.gameObject;
        is_selected = false;
        time_since_creation = 0;
        ParticleSystem particles = this.gameObject.GetComponent<ParticleSystem>();
        var emissions = particles.emission;
        emissions.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        time_since_creation += Time.deltaTime;
        if (time_since_creation >= 0.1f)
        {
            ParticleSystem particles = this.gameObject.GetComponent<ParticleSystem>();
            var emissions = particles.emission;
            emissions.enabled = true;
        }
    }

    void OnMouseDown()
    {
        is_selected = true;
        selected_sat_id = satellite.GetInstanceID();
        Camera_Control.change_target(satellite.GetComponent<Satellite_Orbit>());
        //show stuff
    }
}
