using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class MovableObject : MonoBehaviour
{
    Rigidbody rb;

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
}
