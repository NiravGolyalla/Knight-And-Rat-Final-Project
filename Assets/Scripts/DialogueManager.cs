using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public TMP_Text nameText;
	public TMP_Text dialogueText;
    public Image icon;
	public Animator anim;
    public bool speaking = false;

	private Queue<string> sentences;

    void Awake()
    {
        instance = this;
    }
	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
	}

	public void StartDialogue (Dialogue dialogue)
	{
		anim.SetTrigger("Close");
        anim.ResetTrigger("Open");

		nameText.text = dialogue.name;
        icon.sprite = dialogue.icon;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
        speaking = true;
		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
        speaking = false;
		anim.SetTrigger("Open");
        anim.ResetTrigger("Close");
	}
}
