using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource audioSource;

    private void Start()
    {
        // Initialize the slider value to match the current audio source volume
        volumeSlider.value = audioSource.volume;

        // Add a listener to the slider value change event
        volumeSlider.onValueChanged.AddListener(UpdateAudioVolume);
    }

    private void UpdateAudioVolume(float volume)
    {
        // Set the audio source volume to the value of the slider
        audioSource.volume = volume;
    }


public void musicoff()
    {
        volumeSlider.value = 0;
    }
    public void musicon()
    {
        volumeSlider.value = 0.219f;
    }
}
