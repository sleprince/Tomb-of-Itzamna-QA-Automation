using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private Color highlightColour;

    private List<Material> materials;

    private void Awake()
    {
        materials = new List<Material>();
        foreach (Renderer r in renderers)
        {
            materials.AddRange(new List<Material>(r.materials));
        }
    }

    public void Toggle(bool toggle)
    {
        if (toggle)
        {
            foreach (Material m in materials)
            {
                m.EnableKeyword("_EMISSION");
                m.SetColor("_EmissionColor", highlightColour);
            }
        }
        else
        {
            foreach (Material m in materials)
            {
                m.DisableKeyword("_EMISSION");
            }
        }
    }
}
