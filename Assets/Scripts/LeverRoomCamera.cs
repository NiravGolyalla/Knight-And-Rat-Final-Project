using UnityEngine;
using Cinemachine;
using System.Collections;

public class LeverRoomCamera : MonoBehaviour
{
    public Transform centerOfMap;
    public float cameraPanSpeed = 2f;
    private bool hasEntered = false;
    private CinemachineVirtualCamera cineCamera;

    private void Awake()
    {
        cineCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasEntered && other.CompareTag("Player"))
        {
            hasEntered = true;
            StartCoroutine(PanCameraToCenter());
        }
    }

    private IEnumerator PanCameraToCenter()
    {
        var startingPosition = cineCamera.transform.position;
        var targetPosition = centerOfMap.position;
        var elapsedTime = 0f;

        while (elapsedTime < cameraPanSpeed)
        {
            cineCamera.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / cameraPanSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
