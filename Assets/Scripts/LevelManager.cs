using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Animator transition;
    float transitionTime = 1f;
    public bool FinishTutorial = false;
    public bool FinishDungeon = false;
    public bool DevMode = false;

    [SerializeField] private TMP_Text s;
    public bool lock_ = false;

    public bool reloading = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (DevMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                instance.LoadLevel(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                instance.LoadLevel(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                instance.LoadLevel(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                instance.LoadLevel(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                instance.LoadLevel(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                instance.LoadLevel(7);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                instance.LoadLevel(8);
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
          DevMode = !DevMode;
          StartCoroutine(showDevMode());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public void Reload() { StartCoroutine(reloadScene()); }

    public void LoadLevel(string i)
    {
        StartCoroutine(loadScene(i));
    }
    public void LoadLevel(int i)
    {
        StartCoroutine(loadScene(i));
    }

    IEnumerator loadScene(int i)
    {
        if (!reloading)
        {
            reloading = true;
            transition.CrossFade("SceneChange_Start", 0, 0);
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(i);
            transition.CrossFade("SceneChange_End", 0, 0);
            yield return new WaitForSeconds(transitionTime);
            reloading = false;
        }
        yield return null;
    }

    IEnumerator loadScene(string i)
    {
        if (!reloading)
        {
            reloading = true;
            transition.CrossFade("SceneChange_Start", 0, 0);
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(i);
            transition.CrossFade("SceneChange_End", 0, 0);
            yield return new WaitForSeconds(transitionTime);
            reloading = false;
        }
        yield return null;
    }

    IEnumerator reloadScene()
    {
        if (!reloading)
        {
            reloading = true;
            transition.CrossFade("Fade_Start", 0, 0);
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            transition.CrossFade("Fade_End", 0, 0);
            yield return new WaitForSeconds(transitionTime);
            reloading = false;
        }
        yield return null;
    }

    IEnumerator showDevMode(){
        if(!lock_){
            lock_ = true;
            s.text = "Dev Mode";
            s.text += DevMode ? "Enabled" : "Disabled";
            transition.CrossFade("dev_Start",0,0);
            yield return new WaitForSeconds(0.5f); 
            transition.CrossFade("dev_End",0,0);
            yield return new WaitForSeconds(0.5f);
            lock_ = false;
        }
    }
}
