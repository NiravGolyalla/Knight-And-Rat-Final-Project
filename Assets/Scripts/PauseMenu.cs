using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public bool isPaused = false;
    bool animating = false;
    public Animator anim;
    float transitionTime = 1f;
    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused){
                Paused();
            } else{
                UnPaused();
            }
        }
    }

    public void Paused(){
        isPaused = true;
        Time.timeScale = 0f;    
        anim.SetTrigger("Close");
        anim.ResetTrigger("Open");
    }

    public void UnPaused(){
        isPaused = false;
        Time.timeScale = 1f;    
        anim.SetTrigger("Open");
        anim.ResetTrigger("Close");
    }


}









