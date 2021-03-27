using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this will probably be a fully static class - Will
public class Satellite_Spawner : MonoBehaviour
{
    static int name_id;

    // Start is called before the first frame update
    void Start()
    {
        name_id = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (name_id == 1)   // TEMPORARY FOR TESTING
        {
            // the following code should only run if new satellite button is pressed

            // take a line of input from the .txt and separate it into important variables

            // create a satellite at the correct position
            GameObject new_sat = new GameObject("satellite_" + name_id.ToString()); // create a new satellite with a unique name
            name_id++;      // increment the id so that the next sat has unique name

            // give the satellite a satellite_orbit script with correct variables

            //
            // CREATE THE SATELLITE'S PARTICLE SYSTEM
            //

            new_sat.AddComponent<ParticleSystem>(); // add particle system to sat
            ParticleSystem particle = new_sat.GetComponent<ParticleSystem>();   // get the particle system of the sat

            // set the attributes in the main struct of the particle system
            var main = particle.main;   // struct containing general attributes of the particle system
            main.startLifetime = 20f;   // time before particles go away
            main.startSpeed = -0.01f;   // initial velocity of particles with respect to the front axis
            main.startColor = new Color(255, 0, 0, 255); // red, green, blue, alpha
            main.simulationSpace = ParticleSystemSimulationSpace.World; // whether particles consider the sat or the world center to be (0,0,0)
            main.maxParticles = 10000;  // max particles this particular particle system can have at once

            // set the attributes in the emission struct of the particle system
            var emission = particle.emission;   // struct containing attributes relating to emission of particles
            emission.rateOverTime = 0;  // particles emitted per second
            emission.rateOverDistance = 10; // particles emitted per unit distance

            // set the attributes in the shape struct of the particle system
            var shape = particle.shape; // struct containing attibutes of the physical shape of the particle system
            shape.angle = 0;    // angle of the cone to its point
            shape.radius = 0.01f;   // radius of the cone

            // set the attributes in the ColorOverLifetime struct of the particle system
            var color_over_time = particle.colorOverLifetime;   // struct containing attributes of the particle color over its lifetime
            color_over_time.enabled = true; // enable the colorOverLifetime module
            Gradient grad = new Gradient(); // gradient of color between time 0 and itme 1
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },   // color of gradient (color, time)
                         new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });    // alpha values of gradient (alpha, time)
            color_over_time.color = grad;   // set the color over time to the new gradient

            // set the attributes in the particle system's renderer
            ParticleSystemRenderer particle_renderer = new_sat.GetComponent<ParticleSystemRenderer>();   // component w/ attributes affecting how particles are rendered
            particle_renderer.maxParticleSize = 0.1f;    // maximum size of the particles
            particle_renderer.material = (Material)Resources.Load("unity_builtin_extra");    // no idea if this will work, get default particle material and set it

            //
            // set the material of the new satellite
            //
            Renderer renderer = new_sat.GetComponent<Renderer>();   // the renderer of the satellite
            renderer.material = (Material)Resources.Load("satellite_material"); // set the satellite material using the satellite material from Assets/Resources

            //
            // add a collider to the satellite
            //
            new_sat.AddComponent<SphereCollider>(); // add a collider (essentially a hitbox) to the sat
        }
    }
}
