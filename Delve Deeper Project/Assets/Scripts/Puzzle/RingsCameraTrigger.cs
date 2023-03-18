using UnityEngine;
using Cinemachine;
using StarterAssets;

public class RingsCameraTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera puzzleCam;

    private void Awake()
    {
        RingsPuzzle.OnRingsPuzzleCompleted += OnRingsPuzzleCompleted;
    }

    private void OnDestroy()
    {
        RingsPuzzle.OnRingsPuzzleCompleted -= OnRingsPuzzleCompleted;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThirdPersonController>() != null)
        {
            puzzleCam.gameObject.SetActive(true);
            puzzleCam.Priority = 99;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ThirdPersonController>() != null)
        {
            puzzleCam.gameObject.SetActive(false);
            puzzleCam.Priority = 0;
        }
    }

    void OnRingsPuzzleCompleted()
    {
        puzzleCam.Priority = 0;
        puzzleCam.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
