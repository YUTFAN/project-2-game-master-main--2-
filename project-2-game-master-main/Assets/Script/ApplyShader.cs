using UnityEngine;

public class ApplyShader : MonoBehaviour
{
    public string shaderName = "Dissolve";
    [SerializeField] private float increaseRate = 0.1f;
    [SerializeField] private float maxAmount = 1f;

    public Material material;

    private void Start()
    {
        this.enabled = false;
    }

    public void ActivateEffect()
    {
        if (material == null)
        {
            Shader shader = Shader.Find(shaderName);
            if (shader == null)
            {
                Debug.LogError("Shader not found!");
                return;
            }

            material = new Material(shader);
        }

        // apply new material to GameObject's Renderer component
        material.SetFloat("_Amount", 0f);
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.material = material;
        else Debug.LogError("Renderer component is missing!");
    }

    private void Update()
    {
        if (material == null) return;
        float amount = material.GetFloat("_Amount");
        amount += increaseRate * Time.deltaTime;
        amount = Mathf.Clamp(amount, 0, maxAmount);
        material.SetFloat("_Amount", amount);
    }
}