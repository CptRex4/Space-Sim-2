using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Selector : MonoBehaviour
{
    public static int selected_sat_id = 0;
    GameObject satellite;
    public string sat_name;
    float time_since_creation;

    // Start is called before the first frame update
    void Start()
    {
        satellite = this.gameObject;
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
        selected_sat_id = satellite.GetInstanceID();
        Camera_Control.change_target(satellite.GetComponent<Satellite_Orbit>());
        //show stuff
    }
}
