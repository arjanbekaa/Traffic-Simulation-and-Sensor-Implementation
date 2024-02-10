using UnityEngine;
using System.Collections.Generic;

public class RandomMeshRendererActivator : MonoBehaviour
{
    public List<MeshRenderer> meshRenderers; // List of mesh renderers to activate randomly

    private void OnEnable()
    {
        ActivateRandomMeshRenderer();
    }

    private void ActivateRandomMeshRenderer()
    {
        // Disable all mesh renderers
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = false;
        }

        // Randomly select one mesh renderer to enable
        int randomIndex = Random.Range(0, meshRenderers.Count);
        meshRenderers[randomIndex].enabled = true;
    }
}