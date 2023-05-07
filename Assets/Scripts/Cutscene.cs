using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Manticore manticore;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Cutscene trigger entered."); // Add this line for debugging
        if ((other.CompareTag("Knight") || other.CompareTag("Rat")) && manticore.IsCutsceneActive())
        {
            manticore.EndCutscene();
            Debug.Log("Cutscene ended."); // Add this line for debugging
        }
    }
}
