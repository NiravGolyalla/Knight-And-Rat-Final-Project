using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate6 : MonoBehaviour
{
    public PressurePlateController ppc;
    public Sprite open;
    public Sprite closed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ppc.isActivated())
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = open;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = closed;
            
        }
    }
}
