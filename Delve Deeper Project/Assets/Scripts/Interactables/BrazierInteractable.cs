using UnityEngine;

public class BrazierInteractable : MonoBehaviour, IInteractable
{
    public GameObject m_fire;
    public ParticleSystem m_particles;

    [SerializeField] private bool aMultiBrazier = false;

    public string GetInteractText()
    {
        return "Light Brazier";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        LightBrazier();
    }

    private void LightBrazier()
    {
        if (aMultiBrazier)
        {
            BrazierPuzzle puzzle = GameObject.Find("Puzzle").GetComponent<BrazierPuzzle>();
            puzzle.AddBrazier(this.gameObject);

            if (!m_fire.activeSelf)
            {
                m_fire.SetActive(true);
                m_particles.Play();
            }
            else
                m_fire.SetActive(false);
        }        
    }
}
