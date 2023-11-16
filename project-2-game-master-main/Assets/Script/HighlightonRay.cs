using UnityEngine;

public class HighlightOnRaycast : MonoBehaviour
{
    private Renderer rend;
    private Material originalMaterial;
    private Material glowMaterial;
    private static HighlightOnRaycast lastHighlighted = null;
    private RaycastHit hit;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;

        glowMaterial = new Material(Shader.Find("Custom/EdgeGlowing"));
    }

    private void Update()
    {
        if (HighlightWithRay())
        {
            SetHighlight(true);
        }
        else if (lastHighlighted == this)
        {
            SetHighlight(false);
        }
    }

    bool HighlightWithRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit) && hit.collider.gameObject == this.gameObject;
    }

    void SetHighlight(bool highlight)
    {
        if (highlight)
        {
            if (lastHighlighted != null && lastHighlighted != this)
            {
                lastHighlighted.SetHighlight(false);
            }
            rend.material = glowMaterial; 
            lastHighlighted = this;
        }
        else
        {
            rend.material = originalMaterial; 
            if (lastHighlighted == this)
            {
                lastHighlighted = null;
            }
        }
    }
}
