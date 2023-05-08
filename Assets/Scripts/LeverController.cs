using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour
{
    bool on;
    private bool withinRange;
    public Sprite turnedOff;
    public Sprite turnedOn;
    private BoxCollider2D leverCollider;



    // Start is called before the first frame update
    void Start()
    {
        on = false;
        withinRange = false;
        leverCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (withinRange)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                StartCoroutine(flipLever(true));
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(flipLever(false));
            }
        }
    }

    
    private IEnumerator flipLever(bool didAttack)
    {
        if (didAttack) {yield return new WaitForSeconds(0.5f);
 }
        on = !on;
        if (on)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = turnedOn;
            flipLeverColliderLocation();
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = turnedOff;
            flipLeverColliderLocation();
        }
        yield return null;
    }

    public bool isOn() { return on; }
    private void OnTriggerStay2D(Collider2D other)
    {
        // Checks if the player is in range to activate the lever
        // Only knights can activate levers
        if (other.tag == "Knight")
        {
            withinRange = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Knight")
        {
            withinRange = false;
        }
    }

    private void flipLeverColliderLocation()
    {
        // We want the player actually move to where the lever to flip it
        Vector2 offset = leverCollider.offset;
        offset.x = -offset.x;
        leverCollider.offset = offset;
    }
}
