using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public bool isPaused = false;
    public Animator anim;
    string[] non_pause = {"Start Menu"};

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string active_scene = SceneManager.GetActiveScene().name;
            if(!non_pause.Contains(active_scene)){
                if(!isPaused){
                    Paused();
                } else{
                    UnPaused();
                }
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

    public void LevelSelect(){
        LevelManager.instance.LoadLevel("Sewer Hub");
        UnPaused();
    }

    public void MainMenu(){
        LevelManager.instance.LoadLevel("Start Menu");
        UnPaused();
    }


}









