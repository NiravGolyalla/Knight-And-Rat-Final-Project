using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Manticore manticore;
    public List<Dialogue> dialogue;
    private int start_ = 0;
    private Queue<Dialogue> dialouges;

    void Start(){
        dialouges = new Queue<Dialogue>();
        foreach(Dialogue d in dialogue){
            dialouges.Enqueue(d);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Rat") || other.gameObject.CompareTag("Knight")) && start_ == 0)
        {
            start_ += 1;
        }
    }

    void Update(){
        if(start_ == 1){
            Dialogue d = dialouges.Dequeue();
            DialogueManager.instance.StartDialogue(d);
            start_ += 1;
        }
        if(start_ == 2 && !DialogueManager.instance.speaking){
            Dialogue d = dialouges.Dequeue();
            DialogueManager.instance.StartDialogue(d);
            start_ += 1;
        }
        if(start_ == 3 && !DialogueManager.instance.speaking && PlayerController.instantance.isAttacking){
            Dialogue d = dialouges.Dequeue();
            DialogueManager.instance.StartDialogue(d);
            start_ += 1;
        }
        //If Hes less than half
        if(start_ == 4 && !DialogueManager.instance.speaking){
            Dialogue d = dialouges.Dequeue();
            DialogueManager.instance.StartDialogue(d);
            start_ += 1;
        }
        //1/4 health
        if(start_ == 5 && !DialogueManager.instance.speaking){
            Dialogue d = dialouges.Dequeue();
            DialogueManager.instance.StartDialogue(d);
            start_ += 1;
        }
        //Death
        if(start_ == 6 && !DialogueManager.instance.speaking){
            Dialogue d = dialouges.Dequeue();
            DialogueManager.instance.StartDialogue(d);
            start_ += 1;
        }
        //Princess
        if(start_ == 7 && !DialogueManager.instance.speaking){
            Dialogue d = dialouges.Dequeue();
            DialogueManager.instance.StartDialogue(d);
            start_ += 1;
        }
        if(start_ == 8 && !DialogueManager.instance.speaking){
            // LevelManager.instance.LoadLevel("Start Menu");
        }

        if (DialogueManager.instance.speaking && Input.GetKeyDown(KeyCode.E)){
            DialogueManager.instance.DisplayNextSentence();
        }



        
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log("Cutscene trigger entered."); // Add this line for debugging
    //     if ((other.CompareTag("Knight") || other.CompareTag("Rat")) && manticore.IsCutsceneActive())
    //     {
    //         manticore.EndCutscene();
    //         Debug.Log("Cutscene ended."); // Add this line for debugging
    //     }
    // }
}
