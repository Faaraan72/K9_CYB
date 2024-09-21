using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class clickaudio : MonoBehaviour
{
    public AudioClip tapSound;

    private Button button;
    private AudioSource audioSource;

    private void Start()
    {
        button = GetComponent<Button>();
       audioSource = FindObjectOfType<AudioSource>();

        button.onClick.AddListener(PlayTapSound);
    }

    private void PlayTapSound()
    {
        if (tapSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(tapSound);
        }
    }
}
