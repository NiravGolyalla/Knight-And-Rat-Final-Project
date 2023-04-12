using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{

    public LeverController[] levers;

    public bool[] whichLeversToOpenGate;
    public PressurePlateController[] pressurePlates;

    private int numOfActivatedLevers;
    private int numOfActivatedPressurePlates;

    // Start is called before the first frame update
    void Start()
    {
        numOfActivatedLevers = 0;
        numOfActivatedPressurePlates = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if every lever matches 
        for (int i = 0; i < levers.Length; i++)
        {
            if (levers[i].isOn() == whichLeversToOpenGate[i]) { numOfActivatedLevers++; }
            else { numOfActivatedLevers--; }
        }

        foreach (PressurePlateController pressurePlate in pressurePlates)
        {
            if (pressurePlate.isActivated()) { numOfActivatedPressurePlates++; }
            else { numOfActivatedPressurePlates--; }

        }

        if (numOfActivatedLevers == levers.Length && numOfActivatedPressurePlates == pressurePlates.Length)
        {
            // Open the Gate
        }
        else
        {
            // Close the gate
        }
    }
}
