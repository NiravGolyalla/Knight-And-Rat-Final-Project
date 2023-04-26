using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutRopeController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isCut;

    private bool withinRange;

    private int numOfCuts;

    public bool hasBeenCut() { return isCut; }
    void Start()
    {
        numOfCuts = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && withinRange)
        {
            numOfCuts--;
        }
        isCut = (numOfCuts <= 0);
        if (isCut)
        {

            SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
            Sprite cutRopeSprite = Resources.Load<Sprite>("rope_cut");
            sprite.sprite = cutRopeSprite;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Rat")
        {
            print("oh yea");
            withinRange = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Rat")
        {
            withinRange = false;
        }
    }
}
