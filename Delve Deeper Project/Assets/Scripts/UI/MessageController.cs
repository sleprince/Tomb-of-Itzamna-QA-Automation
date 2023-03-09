using UnityEngine;
using TMPro;
using System.Collections;

public class MessageController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public TextMeshProUGUI messageText;

    protected Coroutine DeactivateCoroutine;
    protected readonly int HashActiveParam = Animator.StringToHash("Active");

    IEnumerator SetAnimParamWthDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool(HashActiveParam, false);
    }

    public void ActivateCanvasWithText(string text)
    {
        if (DeactivateCoroutine != null)
        {
            StopCoroutine(DeactivateCoroutine);
            DeactivateCoroutine = null;
        }

        gameObject.SetActive(true);
        anim.SetBool(HashActiveParam, true);
        messageText.text = text;
    }

    public void DeactivateCanvasWithDelay(float delay)
    {
        DeactivateCoroutine = StartCoroutine(SetAnimParamWthDelay(delay));
    }
}
