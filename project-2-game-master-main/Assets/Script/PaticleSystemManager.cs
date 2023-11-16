using UnityEngine;

public class PlayParticleSystem : MonoBehaviour
{
    private ParticleSystem portal;

    private void Start()
    {
        portal = GetComponent<ParticleSystem>();

        if (portal != null)
        {
            portal.Play();
        }
        else
        {
            Debug.LogWarning("No Particle System found on this GameObject.");
        }
    }
}
