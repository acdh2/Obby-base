using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MaterialPhysicsTag
{
    public Material renderingMaterial;
    public PhysicMaterial physicsMaterial;
    public string tag;
}

public class MaterialPhysicsLinker : MonoBehaviour
{
    [Tooltip("Define the associations between rendering materials, physics materials, and tags.")]
    public List<MaterialPhysicsTag> materialMappings;

    public void ApplyMaterialsAndTags()
    {
        // Iterate over all GameObjects in the hierarchy
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            // Get the MeshRenderer component (if any) of the current GameObject
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer == null) continue;

            // Get the materials used by the renderer
            Material[] objectMaterials = renderer.sharedMaterials;

            foreach (Material objectMaterial in objectMaterials)
            {
                foreach (MaterialPhysicsTag mapping in materialMappings)
                {
                    if (objectMaterial == mapping.renderingMaterial)
                    {
                        // Apply the physics material if there's a collider
                        if (mapping.physicsMaterial != null) {
                            Collider collider = obj.GetComponent<Collider>();
                            if (collider != null)
                            {
                                collider.sharedMaterial = mapping.physicsMaterial;
                            }
                        }

                        // Apply the tag
                        if (mapping.tag != null && mapping.tag != "") {
                            obj.tag = mapping.tag;
                        }

                        // Exit the loop since we've found the matching material
                        break;
                    }
                }
            }
        }
    }

    // Optional: Call this automatically on start
    private void Start()
    {
        ApplyMaterialsAndTags();
    }
}
