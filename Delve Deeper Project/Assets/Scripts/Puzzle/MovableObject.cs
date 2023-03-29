using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class MovableObject : MonoBehaviour
{
    Rigidbody rb;
    CharacterController playerController;

    AudioSource audioSource;
    [SerializeField] private AudioClip movingSound;

    [SerializeField] private Transform target;
    [SerializeField] private float speed = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerController = FindObjectOfType<CharacterController>();

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

    public void MoveObject(TriggerInteractable trigger)
    {
        trigger.transform.parent.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        playerController.enabled = false;
        float origY = playerController.transform.position.y;
        playerController.transform.position = new Vector3(trigger.transform.position.x, origY, trigger.transform.position.z);
        playerController.transform.forward = trigger.transform.forward;
        playerController.enabled = true;
    }
}
