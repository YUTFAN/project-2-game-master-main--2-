using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider volumeSlider;

    private void Start()
    {
        mixer.GetFloat("Volume", out float currentVolume);
        volumeSlider.value = Mathf.Pow(10, currentVolume / 20);
    }

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        mixer.SetFloat("Volume", volume );
    }
}
