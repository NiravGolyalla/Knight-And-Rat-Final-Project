using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSigController : MonoBehaviour
{

    public Sprite turnedOffSprite;
    public Sprite turnedOnSprite;

    public LeverController lever;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = turnedOffSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (lever.isOn())
        {
            sr.sprite = turnedOnSprite;
        }
        else
        {
            sr.sprite = turnedOffSprite;
        }
    }
}
