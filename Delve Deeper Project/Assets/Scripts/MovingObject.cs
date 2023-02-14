using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class MovingObject : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
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
}
