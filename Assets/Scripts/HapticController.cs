using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HapticController : MonoBehaviour
{
    private HapticMaterial hapticMaterial;
    public GameObject hapticFieldPrefab;
    
    // Define the low and high viscosity forces
    public Force lowViscosity = new Force(0.5f, 0.5f, 0.5f, 10.0f, 0.2f, 0.0f, 0.0f, Vector3.zero, 0.0f, Vector3.zero, 0.0f, 0.0f);
    public Force highViscosity = new Force(1.0f, 1.0f, 1.0f, 20.0f, 0.9f, 0.0f, 0.0f, Vector3.zero, 0.0f, Vector3.zero, 0.0f, 0.0f);
    
    public string ForceDisplayText = "";
    public Text ForceDisplay;
    private int forceCount = 0; // Used to toggle between low and high viscosity

    // List to store forces
    private List<Force> forceList;

    void Start()
    {
        // Initialize the hapticMaterial from prefab and set up the list of forces
        hapticMaterial = hapticFieldPrefab.GetComponent<HapticMaterial>();
        forceList = new List<Force> { lowViscosity, highViscosity }; // Forces to toggle between
    }

    void Update()
    {
        // Toggle between low and high viscosity when the "p" key is pressed
        if (Input.GetKeyDown("p"))
        {
            ChangeForce(forceList[forceCount]);
            DisplayForce(forceCount == 0 ? "Low Viscosity" : "High Viscosity");
            
            // Toggle between the forces
            forceCount = (forceCount + 1) % forceList.Count; 
        }
    }

    // Method to apply the selected force to the hapticMaterial
    public void ChangeForce(Force force)
    {
        hapticMaterial.hMass = force.mass;
        hapticMaterial.hStiffness = force.stiffness;
        hapticMaterial.hDamping = force.damping;
        hapticMaterial.hImpulseD = force.impulseDepth;
        hapticMaterial.hViscosity = force.viscosity;
        hapticMaterial.hFrictionS = force.staticFriction;
        hapticMaterial.hFrictionD = force.dynamicFriction;
        hapticMaterial.hConstForceDir = force.constantForceDir;
        hapticMaterial.hConstForceMag = force.constantForceMag;
        hapticMaterial.hSpringDir = force.springDir;
        hapticMaterial.hSpringMag = force.springMagnitude;
        hapticMaterial.hPopthAbs = force.popThroughForce;
    }

    // Method to update the UI text displaying the current force type
    void DisplayForce(string forceName)
    {
        // Update the UI with the name of the current force
        ForceDisplay.text = forceName;
    }
}

// Force class to represent different physical force properties
public class Force
{
    public float mass;
    public float stiffness;
    public float damping;
    public float impulseDepth;
    public float viscosity;
    public float staticFriction;
    public float dynamicFriction;
    public Vector3 constantForceDir;
    public float constantForceMag;
    public Vector3 springDir;
    public float springMagnitude;
    public float popThroughForce;

    // Constructor to initialize the Force properties
    public Force(float mass, float stiffness, float damping,
                 float impulseDepth, float viscosity,
                 float staticFriction, float dynamicFriction,
                 Vector3 constantForceDir, float constantForceMag,
                 Vector3 springDir, float springMagnitude,
                 float popThroughForce)
    {
        this.mass = mass;
        this.stiffness = stiffness;
        this.damping = damping;
        this.impulseDepth = impulseDepth;
        this.viscosity = viscosity;
        this.staticFriction = staticFriction;
        this.dynamicFriction = dynamicFriction;
        this.constantForceDir = constantForceDir;
        this.constantForceMag = constantForceMag;
        this.springDir = springDir;
        this.springMagnitude = springMagnitude;
        this.popThroughForce = popThroughForce;
    }
}
