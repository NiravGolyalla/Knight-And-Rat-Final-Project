using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Manticore manticore;
    public List<Dialogue> dialogue;
    private int start_ = 0;
    private Queue<Dialogue> dialouges;
    private bool fightStarted = false;
    private bool lastSentenceShown = false;
    public FallingBarrelSpawner fallingBarrelSpawner;

    void Start()
    {
        dialouges = new Queue<Dialogue>();
        foreach (Dialogue d in dialogue)
        {
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

    void Update()
    {
        if (start_ >= 1 && start_ <= 3)
        {
            if (!DialogueManager.instance.speaking && dialouges.Count > 0)
            {
                Dialogue d = dialouges.Dequeue();
                DialogueManager.instance.StartDialogue(d);
                start_ += 1;
            }
        }
        else if (start_ == 4 && !DialogueManager.instance.speaking && !fightStarted && dialouges.Count > 0)
        {
            if (PlayerController.instantance.isAttacking)
            {
                Dialogue d = dialouges.Dequeue();
                DialogueManager.instance.StartDialogue(d);
                start_ += 1;
                lastSentenceShown = true;
            }
        }
        else if (start_ == 5 && lastSentenceShown && !DialogueManager.instance.speaking && !fightStarted)
        {
            fightStarted = true;
            // Start the fight here
            manticore.StartFight();
            fallingBarrelSpawner.StartSpawningBarrels();
        }
        else if (fightStarted)
        {
            // Check Manticore's health and trigger dialogues based on health
            if (manticore.health <= manticore.health / 2 && start_ == 6 && !DialogueManager.instance.speaking && dialouges.Count > 0)
            {
                Dialogue d = dialouges.Dequeue();
                DialogueManager.instance.StartDialogue(d);
                start_ += 1;
            }
            else if (start_ == 7 && !DialogueManager.instance.speaking && dialouges.Count > 0)
            {
                Dialogue d = dialouges.Dequeue();
                DialogueManager.instance.StartDialogue(d);
                start_ += 1;
            }
            else if (start_ == 8 && !DialogueManager.instance.speaking && dialouges.Count > 0)
            {
                Dialogue d = dialouges.Dequeue();
                DialogueManager.instance.StartDialogue(d);
                start_ += 1;
            }
            else if (start_ == 9 && !DialogueManager.instance.speaking && dialouges.Count > 0)
            {
                // LevelManager.instance.LoadLevel("Start Menu");
            }
        }

        if (DialogueManager.instance.speaking && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.instance.DisplayNextSentence();
        }
    }

    public void TriggerCutscene()
    {
        if (start_ == 0)
        {
            start_ += 1;
        }
    }
}
