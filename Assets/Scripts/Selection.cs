using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{

    public static GameObject selected;
    public GameObject nul;

    // Start is called before the first frame update
    void Start()
    {
        selected = nul;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void new_selection(GameObject sat)
    {
        selected = sat;
    }
}
