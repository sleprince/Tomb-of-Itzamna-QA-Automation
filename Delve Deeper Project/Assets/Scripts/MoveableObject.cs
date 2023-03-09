using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class MoveableObject : MonoBehaviour, IInteractable
{
    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] private string interactText;
    [SerializeField] private AudioClip movingSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = movingSound;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude >= 0.1 && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void Interact(Transform interactorTransform)
    {
        //Debug.Log("Statue");
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
