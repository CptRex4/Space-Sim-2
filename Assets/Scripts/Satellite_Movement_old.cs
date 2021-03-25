using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_Movement : MonoBehaviour
{
    public Vector3 velocety;
    float distance_to_earth;

    float earth_mass = 5.972f * Mathf.Pow(10, 24);
    float gravitation_constant = 6.673f * Mathf.Pow(10, -11);
    Vector3 pos;
    Vector3 unit_to_earth;

    public GameObject earth;


    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.Find("Earth");
        //earth_pos = earth.GetComponent<Transform>().position;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        unit_to_earth = Unit_Vector();
        float acceleration = gravitation_constant * earth_mass / Mathf.Pow(Vector3.Distance(earth.transform.position, transform.position), 2); // a = Gm/d^2

        velocety.x += unit_to_earth.x * acceleration * Time.deltaTime; // Calculates the new velocity
        velocety.y += unit_to_earth.y * acceleration * Time.deltaTime;
        velocety.z += unit_to_earth.z * acceleration * Time.deltaTime;

        // Calculates the new position
        pos.x += (velocety.x * Time.deltaTime); //+ (0.5f * unit_to_earth.x * acceleration * Time.deltaTime);
        pos.y += (velocety.y * Time.deltaTime); //+ (0.5f * unit_to_earth.y * acceleration * Time.deltaTime);
        pos.z += (velocety.z * Time.deltaTime); //+ (0.5f * unit_to_earth.z * acceleration * Time.deltaTime);

        transform.position = pos;

    }

    // Creates the unit vector pointed from the satellite to earth
    Vector3 Unit_Vector()
    {
        Vector3 dist_vector; // x, y, z, components of distance to earth from satellite
        dist_vector.x = earth.transform.position.x - transform.position.x;
        dist_vector.y = earth.transform.position.y - transform.position.y;
        dist_vector.z = earth.transform.position.z - transform.position.z;
        //print(dist_vector);

        distance_to_earth = Vector3.Distance(earth.transform.position, transform.position);

        Vector3 unit_vector; // x, y, z, components of unit vector that points to earth
        unit_vector.x = dist_vector.x / distance_to_earth;
        unit_vector.y = dist_vector.y / distance_to_earth;
        unit_vector.z = dist_vector.z / distance_to_earth;
        print(unit_vector);
        return unit_vector;
    }
}