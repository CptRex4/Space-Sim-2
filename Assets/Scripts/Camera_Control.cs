using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    GameObject cam;     // camera object
    float theta;        // degrees from 0 (xz)
    float phi;          // degrees from -90 (y)
    static float distance;     // distance between camera and camera focus point
    Vector3 mouse_pos;  // position of the mouse during last frame
    float x_change;     // difference in x between mouse pos in last frame and current mouse pos
    float y_change;
    Vector3 pos;        // to be set to new position of camera
    static bool sat_selected;  // if a satellite is currently selected
    static Satellite_Orbit current_sat;  // satellite id camera is focused on
    static Vector3 sat_pos; // pos of satellite camera is focused on
    Vector3 scale;

    public float min_distance = 5;
    public float max_distance = 20;
    public float rotation_speed = 1;
    static public float initial_distance = 120;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");   // set cam to a reference of the main camera
        theta = 0.0f;       // xz angle of camera
        phi = 90.0f;        // vertical angle of camera
        distance = initial_distance;   // radial distance of camera from (0, 0, 0) initial value
        x_change = 0.0f;    // no x_change on start
        y_change = 0.0f;    // no y_change on start
        Vector3 pos_init;
        pos_init.x = distance;
        pos_init.y = 0;
        pos_init.z = 0;
        cam.transform.position = pos_init;
        cam.transform.LookAt(Vector3.zero);
        sat_selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance -= 10 * Input.mouseScrollDelta.y;
        if (distance < min_distance)
            distance = min_distance;
        if (distance > max_distance)
            distance = max_distance;

        if (!sat_selected)
        {
            

            if (Input.GetKeyDown(KeyCode.Mouse1))   // runs only once after rmb is held
            {
                mouse_pos = Input.mousePosition;    // initial position of mouse as rmb starts being held
            }
            if (Input.GetKey(KeyCode.Mouse1))       // runs every frame while rmb is held
            {
                x_change = mouse_pos.x - Input.mousePosition.x; // get the x-difference between now and last frame of mouse
                theta += rotation_speed * x_change;  // add the x-diffence to theta

                y_change = mouse_pos.y - Input.mousePosition.y; // get the y-difference between now and last frame of mouse
                phi += rotation_speed * y_change;   // add the y-difference to phi

                if (phi > 175)
                    phi = 175;
                if (phi < 5)
                    phi = 5;

                mouse_pos = Input.mousePosition;    // get this frame's mous pos in preparation for next frame
            }

            // use spherical coordinate to cartesian coordinate formulas to get the new xyz position of the camera
            pos.x = distance * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Cos(Mathf.Deg2Rad * theta);    // x = rsin(phi)cos(theta)
            pos.z = distance * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Sin(Mathf.Deg2Rad * theta);    // z = rsin(phi)sin(theta) (y is up instead of z here)
            pos.y = -distance * Mathf.Cos(Mathf.Deg2Rad * phi);                                      // y = rcos(phi)

            cam.transform.position = pos;   // set the camera's position
            cam.transform.LookAt(Vector3.zero); // set the camera to look at (0, 0, 0)
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                sat_selected = false;
                mouse_pos = Input.mousePosition;    // initial position of mouse as rmb starts being held
            }
            
            cam.transform.LookAt(Vector3.zero);

            pos = current_sat.pos;
            scale.Set(distance, distance, distance);
            scale.Set(1.2f, 1.2f, 1.2f);
            pos.Scale(scale);
            cam.transform.position = pos;

            if (pos.magnitude == 0)
                phi = 0;
            else
                phi = (Mathf.PI - Mathf.Acos(pos.y / pos.magnitude)) * Mathf.Rad2Deg;

            if (pos.x == 0)
                theta = 0;
            else
                theta = (Mathf.Rad2Deg * Mathf.Atan2(pos.z, pos.x));
            

        }
    }

    public static void change_target(Satellite_Orbit sat)
    {
        sat_selected = true;
        current_sat = sat;
    }
}
