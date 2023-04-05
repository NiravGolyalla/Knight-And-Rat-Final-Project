using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public WallController wallController;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wallController.SetMoving(false);
            StartCoroutine(wallController.MoveWallsBack());
        }
    }
}
