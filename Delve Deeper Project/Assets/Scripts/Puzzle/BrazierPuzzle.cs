using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrazierPuzzle : MonoBehaviour
{
    [SerializeField] private GameEvent orderCorrect;
    public List<GameObject> correctOrder = new List<GameObject>();
    public List<GameObject> selectedOrder = new List<GameObject>();

    bool statusChecked = false;
    bool litOrderCorrect = true;

    public static UnityAction OnBrazierPuzzleCompleted;

    public void AddBrazier(GameObject newBrazier)
    {
        if (selectedOrder.Contains(newBrazier)) 
            selectedOrder.Remove(newBrazier);
        else
            selectedOrder.Add(newBrazier);
    }

    private void Update()
    {
        if (correctOrder.Count > 0)
        {
            if (!statusChecked)
            {
                if (selectedOrder.Count == correctOrder.Count)
                {
                    CheckStatus();
                }
            }
        }        
    }

    void CheckStatus()
    {
        Debug.Log("Lists match");
        statusChecked = true;
        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (correctOrder[i] != selectedOrder[i]) 
            {
                litOrderCorrect = false;
                StartCoroutine(WrongOrderSelection());
                return;
            }
        }

        if (litOrderCorrect)
        {
            StartCoroutine(RightOrderSelection());
            OnBrazierPuzzleCompleted?.Invoke();
        }
    }

    IEnumerator WrongOrderSelection()
    {
        yield return new WaitForSeconds(1.5f);

        foreach(GameObject brazier in selectedOrder)
        {
            brazier.GetComponent<BrazierInteractable>().m_fire.SetActive(false);
        }
        litOrderCorrect = true;
        selectedOrder.Clear();
        statusChecked = false;
    }

    IEnumerator RightOrderSelection()
    {
        yield return new WaitForSeconds(1.5f);

        orderCorrect.Raise(this.gameObject);
        foreach (GameObject brazier in selectedOrder)
        {
            Destroy(brazier.GetComponent<Collider>());
        }
    }
}
