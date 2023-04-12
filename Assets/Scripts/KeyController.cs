using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private bool acquired;
    private SpriteRenderer sprite;

    public GameObject[] goals;
    private bool[] goals_complete;
    // Start is called before the first frame update
    
    public bool isAcquired() { return acquired; }
    
    void Start()
    {
        acquired = false;
        sprite = gameObject.GetComponent<SpriteRenderer>();

        goals_complete = new bool[goals.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (acquired)
        {
            sprite.sprite = null;
        }

        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i].GetComponent<CutRopeController>() != null)
            {
                goals_complete[i] = goals[i].GetComponent<CutRopeController>().hasBeenCut();
            }
            else if (goals[i].GetComponent<LeverController>() != null)
            {
                goals_complete[i] = goals[i].GetComponent<LeverController>().isOn();
            }
        }
        
        Transform cageTransform = transform.Find("Cage");
        BoxCollider2D cage = cageTransform.GetComponent<BoxCollider2D>();
        SpriteRenderer cage_sprite = cageTransform.GetComponent<SpriteRenderer>();

        if (hasAllGoalsBeenCompleted())
        {   
            cage.isTrigger = true;
            cage_sprite.sprite = null;
        }
        else
        {
            cage.isTrigger = false;
            cage_sprite.sprite = Resources.Load<Sprite>("1_standard/Cage_grey_standard_hanging");
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        acquired = true;
    }

    bool hasAllGoalsBeenCompleted() {
        for (int i = 0; i < goals_complete.Length; i++)
        {
            if (!goals_complete[i])
            {
                return false;
            }
        }
        return true;
    }
}
