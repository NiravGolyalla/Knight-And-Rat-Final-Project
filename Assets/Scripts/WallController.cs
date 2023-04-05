using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class WallController : MonoBehaviour
{
    public Tilemap leftWall;
    public Tilemap rightWall;

    private bool moving = true;
    private Vector3 leftWallStartPosition;
    private Vector3 rightWallStartPosition;
    private float moveSpeed = 1f; // adjust as needed
    private float moveBackSpeed = 0.5f; // half the move speed
    private float moveBackDuration;

    public void Start()
    {
        leftWallStartPosition = leftWall.transform.position;
        rightWallStartPosition = rightWall.transform.position;
    }

    public void Update()
    {
        if (moving)
        {
            MoveWalls(Vector3.right * Time.deltaTime);
        }
    }

    private void MoveWalls(Vector3 direction)
    {
        leftWall.transform.Translate(direction);
        rightWall.transform.Translate(-direction);
    }

    public void SetMoving(bool value)
    {
        moving = value;
        if (!moving) // If the walls need to move back to their original positions
        {
            StartCoroutine(MoveWallsBack());
        }
    }

    public IEnumerator MoveWallsBack()
    {
        float distance = Vector3.Distance(leftWall.transform.position, leftWallStartPosition);
        moveBackDuration = distance / moveBackSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < moveBackDuration)
        {
            float step = moveBackSpeed * Time.deltaTime;
            leftWall.transform.position = Vector3.MoveTowards(leftWall.transform.position, leftWallStartPosition, step);
            rightWall.transform.position = Vector3.MoveTowards(rightWall.transform.position, rightWallStartPosition, step);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        leftWall.transform.position = leftWallStartPosition;
        rightWall.transform.position = rightWallStartPosition;
    }


}
