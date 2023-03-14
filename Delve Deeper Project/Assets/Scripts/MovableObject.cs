using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class MovableObject : MonoBehaviour, IInteractable
{
    Rigidbody rb;
    [SerializeField] private string interactText;

    AudioSource audioSource;
    [SerializeField] private AudioClip movingSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
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
