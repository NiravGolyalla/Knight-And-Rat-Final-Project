using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class LevelChange : MonoBehaviour
{
    public string nextLevel = "New_Tutorial_2";
    [SerializeField] bool endTutorial;
    [SerializeField] bool endDungeon;

    [SerializeField] bool toBoss;
    [SerializeField] bool toDungeon;
    [SerializeField] Popup p;




    public void ChangeLevel(string Level){
        LevelManager.instance.LoadLevel(Level);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Rat") || other.gameObject.CompareTag("Knight"))
        {
            if(endTutorial){
                LevelManager.instance.FinishTutorial = true;
            }
            if(endDungeon){
                LevelManager.instance.FinishDungeon = true;
            }
            if(toBoss){
                if(!LevelManager.instance.FinishDungeon){
                    p.active = true;
                    return;
                }
            }
            if(toDungeon){
                if(!LevelManager.instance.FinishTutorial){
                    p.active = true;
                    return;
                }
            } else{
                p.active = false;
            }
            LevelManager.instance.LoadLevel(nextLevel);
        }
    }
    public void Pause(){
        PauseMenu.instance.Paused();
    }
}
