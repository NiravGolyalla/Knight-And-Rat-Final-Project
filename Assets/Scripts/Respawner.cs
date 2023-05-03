using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public GameObject item;
    public GameObject poof;
    public bool reseter = false;
    bool respawning = false;
    GameObject curr = null;

    // Update is called once per frame
    void Update()
    {
        if(!reseter){
            if(curr == null){
                StartCoroutine(Delay());
            }
        } else{
            float dis = Vector2.Distance(PlayerController.instantance.transform.position,transform.position);
            if (dis > 10f && !curr.GetComponent<BarrelController>().solved){
                if(curr != null){
                    Destroy(curr);
                }
                curr = Instantiate(item, transform.position, Quaternion.identity);
            }
        }
        
    }

    IEnumerator Delay(){
        if(!respawning){
            respawning = true;
            yield return new WaitForSeconds(5f);
            Instantiate(poof, transform.position, Quaternion.identity);
            curr = Instantiate(item, transform.position, Quaternion.identity);
            respawning = false;
        }
        yield return null;
    }
}
