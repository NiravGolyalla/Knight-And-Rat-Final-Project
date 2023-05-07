using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatnipController : MonoBehaviour
{
    public bool done = false;
    void Update()
    {
        if(done){
            Destroy(gameObject);
        }
    }

}