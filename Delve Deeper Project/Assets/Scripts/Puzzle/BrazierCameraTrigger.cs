using Cinemachine;
using StarterAssets;
using UnityEngine;

public class BrazierCameraTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera brazierCam;

    private void Awake()
    {
        BrazierPuzzle.OnBrazierPuzzleCompleted += OnBrazierPuzzleCompleted;
    }

    private void OnDestroy()
    {
        RingsPuzzle.OnRingsPuzzleCompleted -= OnBrazierPuzzleCompleted;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThirdPersonController>() != null)
        {
            brazierCam.gameObject.SetActive(true);
            brazierCam.Priority = 99;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ThirdPersonController>() != null)
        {
            brazierCam.gameObject.SetActive(false);
            brazierCam.Priority = 0;
        }
    }

    void OnBrazierPuzzleCompleted()
    {
        brazierCam.Priority = 0;
        brazierCam.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
