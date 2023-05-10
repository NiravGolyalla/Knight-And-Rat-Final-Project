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

    public void Hover() {
        FindAnyObjectByType<AudioManager>().Play("Hover");
    }

    public void Paused(){
        FindAnyObjectByType<AudioManager>().Play("Pause");
        isPaused = true;
        Time.timeScale = 0f;    
        anim.SetTrigger("Close");
        anim.ResetTrigger("Open");
    }

    public void UnPaused(){
        FindAnyObjectByType<AudioManager>().Play("UnPause");
        isPaused = false;
        Time.timeScale = 1f;    
        anim.SetTrigger("Open");
        anim.ResetTrigger("Close");
    }

    public void Resume() {
        FindAnyObjectByType<AudioManager>().Play("Confirm");
        isPaused = false;
        Time.timeScale = 1f;    
        anim.SetTrigger("Open");
        anim.ResetTrigger("Close");
    }

    public void LevelSelect(){
        FindAnyObjectByType<AudioManager>().Play("Confirm");
        LevelManager.instance.LoadLevel("Sewer Hub");
        UnPaused();
    }

    public void MainMenu(){
        FindAnyObjectByType<AudioManager>().Play("Confirm");
        LevelManager.instance.LoadLevel("Start Menu");
        UnPaused();
    }


}









