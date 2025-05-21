using UnityEngine;

public class DiggingEffectController : MonoBehaviour
{
    [SerializeField] ParticleSystem largeCubes;
    [SerializeField] ParticleSystem smallCubes;

    void Start()
    {
        // Optionally stop both particle systems initially
        largeCubes.Stop();
        smallCubes.Stop();
    }

    public void StartDiggingEffect()
    {
        // Play both systems simultaneously
        if (!largeCubes.isPlaying)
            largeCubes.Play();
        if (!smallCubes.isPlaying)
            smallCubes.Play();
    }

    public void StopDiggingEffect()
    {
        // Stop both systems
        if (largeCubes.isPlaying)
            largeCubes.Stop();
        if (smallCubes.isPlaying)
            smallCubes.Stop();
    }
}
