using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    void Awake(){
        instance = this;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
             SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
             SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
             SceneManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
             SceneManager.LoadScene(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
             SceneManager.LoadScene(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
             SceneManager.LoadScene(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
             SceneManager.LoadScene(6);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
             Reload();
        }
    }

    public void Reload(){SceneManager.LoadScene(SceneManager.GetActiveScene().name);}
    
}
