using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStart : MonoBehaviour {

	public Dialogue dialogue;
    [SerializeField]private bool isTargetInTrigger;
    [SerializeField]private Canvas mess;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Rat") || other.gameObject.CompareTag("Knight"))
        {
            isTargetInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Rat") || other.gameObject.CompareTag("Knight"))
        {
            if(DialogueManager.instance.speaking){
                DialogueManager.instance.EndDialogue();
            }
            isTargetInTrigger = false;
        }
    }

    void Update(){
        if(isTargetInTrigger){
            mess.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)){
                if(DialogueManager.instance.speaking){
                    DialogueManager.instance.DisplayNextSentence();
                } else {
                    DialogueManager.instance.StartDialogue(dialogue);
                }
            }
        } else{
            mess.gameObject.SetActive(false);
        }
    }




}