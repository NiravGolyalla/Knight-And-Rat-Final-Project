using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    [SerializeField] LayerMask p; 
    public bool active = true;
    [SerializeField]private float range;
    [SerializeField]private bool isTargetInTrigger;
    [SerializeField]private Canvas mess;
    public string message;
    [SerializeField]private TMP_Text text;
    [SerializeField]private Transform par;



    void Update(){
        text.text = message;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range,p);
        isTargetInTrigger = colliders.Length > 0;
        if(isTargetInTrigger && active){
            mess.gameObject.SetActive(true);
        } else{
            mess.gameObject.SetActive(false);
        }
        transform.rotation = Quaternion.Euler(0f,0f,-1f*par.rotation.z);
        // transform.LookAt(transform.position + Vector3.down);
    }
}
