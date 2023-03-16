using Cinemachine;
using StarterAssets;
using UnityEngine;

public class BrazierCameraTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera puzzleCam;

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

    void OnBrazierPuzzleCompleted()
    {
        puzzleCam.Priority = 0;
        puzzleCam.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
