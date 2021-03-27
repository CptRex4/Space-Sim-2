using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun_Light : MonoBehaviour
{
    public GameObject sun;
    public float distance;

    Sun_Script sun_srpt;
    Vector3 pos;
    float phi;

    // Start is called before the first frame update
    void Start()
    {
        sun_srpt = sun.GetComponent<Sun_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Vector3.zero);

        phi = sun_srpt.phi;
        pos.x = distance * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Cos(Mathf.Deg2Rad * 180);
        pos.y = distance * Mathf.Cos(Mathf.Deg2Rad * phi);
        pos.z = distance * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Sin(Mathf.Deg2Rad * 180);

        transform.position = pos;
    }
}
