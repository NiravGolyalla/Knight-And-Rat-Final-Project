using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    public void ChangeLevel(string Level){
        LevelManager.instance.LoadLevel(Level);
    }
}
