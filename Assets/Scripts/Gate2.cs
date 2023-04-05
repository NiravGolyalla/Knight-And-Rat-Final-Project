using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate2 : MonoBehaviour
{
    public LeverController l1;
    public LeverController l2;
    public LeverController l3;
    public Sprite open;
    public Sprite closed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (l1.isOn() && l2.isOn() && !l3.isOn())
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = open;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = closed;
            
        }
    }
}
