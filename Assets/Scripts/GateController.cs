using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{

    public LeverController[] levers;

    public bool[] whichLeversToOpenGate;
    public PressurePlateController[] pressurePlates;
    public KeyController[] keys;

    public Sprite gate_open;
    public Sprite gate_closed;

    public CutRopeController[] ropes;
    private BoxCollider2D gate;
    private SpriteRenderer gate_sprite;


    // Start is called before the first frame update
    void Start()
    {
        gate = gameObject.GetComponent<BoxCollider2D>();
        gate_sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAllGoalsBeenCompleted())
        {
            gate.isTrigger = true;
            gate_sprite.sprite = gate_open;
           
        }
        else
        {
            gate.isTrigger = false; 
            gate_sprite.sprite = gate_closed;
        }

    }

    private bool hasAllGoalsBeenCompleted()
    {
        // Check if every lever matches 
        for (int i = 0; i < levers.Length; i++)
        {
            if (levers[i].isOn() != whichLeversToOpenGate[i]) return false;
        }

        foreach (PressurePlateController pressurePlate in pressurePlates)
        {
            if (!pressurePlate.isActivated()) return false;

        }
        foreach (KeyController key in keys)
        {
            if (!key.isAcquired()) return false;
        }

        foreach (CutRopeController rope in ropes)
        {
            if (!rope.hasBeenCut()) return false;
        }
        return true;
    }
}
