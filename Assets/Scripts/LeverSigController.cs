using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSigController : MonoBehaviour
{

    public Sprite turnedOffSprite;
    public Sprite turnedOnSprite;

    public LeverController lever;
    public PressurePlateController pre;
    [SerializeField] private bool pred = false;
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
        if(pred){
            if(pre.activate){
                sr.sprite = turnedOnSprite;
            }
            else
            {
                sr.sprite = turnedOffSprite;
            }
        } else{
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
}
