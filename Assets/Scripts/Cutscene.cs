using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Manticore manticore;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Knight") || other.CompareTag("Rat")) && manticore.IsCutsceneActive())
        {
            manticore.EndCutscene();
        }
    }
}
