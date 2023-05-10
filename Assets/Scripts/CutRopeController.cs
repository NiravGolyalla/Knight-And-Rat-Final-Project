using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutRopeController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isCut;

    private bool withinRange;

    private int numOfCuts;

    [SerializeField] private Popup message;
    [SerializeField] private Sprite cutRopeSprite ;
    public bool hasBeenCut() { return isCut; }
    void Start()
    {
        numOfCuts = 1;
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
            
            sprite.sprite = cutRopeSprite;
            message.active = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Rat")
        {
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
