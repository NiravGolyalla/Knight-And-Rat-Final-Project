using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    bool on = false;
    public Sprite turnedOffSprite;
    public Sprite turnedOnSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (on)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = turnedOnSprite;
            } else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = turnedOffSprite;
            }
            on = !on;
        }
    }

    private void OnTriggerEnter(Collider other) {
        print("collided!");
    }
}
