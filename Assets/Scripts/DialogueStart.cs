using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStart : MonoBehaviour {

	[SerializeField] LayerMask p; 
    public Dialogue dialogue;
    bool wasTalking = false;

    float cooldown = 0f;
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f,p);
        isTargetInTrigger = colliders.Length > 0;
        if(isTargetInTrigger){
            mess.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && cooldown == 0f){
                if(DialogueManager.instance.speaking){
                    DialogueManager.instance.DisplayNextSentence();
                } else {
                    DialogueManager.instance.StartDialogue(dialogue);
                }
                wasTalking = true;
                cooldown = 0.3f;
            }
        } else{
            if(DialogueManager.instance.speaking && wasTalking){
                DialogueManager.instance.EndDialogue();
            }
            wasTalking = false;
            mess.gameObject.SetActive(false);
        }
        cooldown -= (cooldown > 0) ? Time.deltaTime : cooldown;
        
    }




}