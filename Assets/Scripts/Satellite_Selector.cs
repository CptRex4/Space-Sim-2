using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Selector : MonoBehaviour
{
    GameObject satellite;

    // Start is called before the first frame update
    void Start()
    {
        satellite = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        Camera_Control.change_target(satellite.GetComponent<Satellite_Orbit>());
    }
}
