using System.Collections;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public float TimeToBreak = 2.0f;

    private float breakTime = 0.0f;
    private bool isBreaking = false;
    private Material platformMaterial;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            platformMaterial = renderer.material;
        }
        else
        {
            Debug.LogWarning("Renderer not found on this GameObject.");
        }
    }

    public void StartBreaking()
    {
        if (!isBreaking)
        {
            isBreaking = true;
            breakTime = TimeToBreak;
        }
    }

    private void Update()
    {
        if (breakTime > 0.0f)
        {
            breakTime -= Time.deltaTime;
            SetAlpha(breakTime / TimeToBreak);
            if (breakTime <= 0.0f)
            {
                StartCoroutine(Break());
            }
        }
    }

    private IEnumerator Break()
    {
        GetComponent<Collider>().enabled = false;

        if (platformMaterial != null)
        {
            SetAlpha(0.0f);
        }

        yield return new WaitForSeconds(2);

        isBreaking = false;
        GetComponent<Collider>().enabled = true;

        if (platformMaterial != null)
        {
            SetAlpha(1.0f);
        }

        print("done");
    }

    public void SetAlpha(float alpha)
    {
        if (platformMaterial != null)
        {
            if (platformMaterial.HasProperty("_Color"))
            {
                Color color = platformMaterial.color;
                color.a = Mathf.Clamp01(alpha); 
                platformMaterial.color = color;
            }
        }
    }
}
