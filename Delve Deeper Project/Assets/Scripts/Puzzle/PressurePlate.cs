using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameEvent pressedEvent;
    public GameEvent releasedEvent;
    Animator anim;
    [SerializeField] private float waitTime;
    [SerializeField] private bool needsWeight;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("Pressed");
        StartCoroutine(TriggerDoor());
    }

    IEnumerator TriggerDoor()
    {
        yield return new WaitForSeconds(waitTime);
        pressedEvent.Raise(this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (needsWeight)
        {
            anim.SetTrigger("Pressed");
            releasedEvent.Raise(this.gameObject);
        }
    }
}
