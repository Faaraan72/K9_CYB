using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class clickaudio : MonoBehaviour
{
    public AudioClip tapSound;

    private Button button;
    private AudioSource audioSource;

    private void Update()
    {
        if(button==null || audioSource == null)
        {
            button = gameObject.GetComponent<Button>();
            audioSource = FindObjectOfType<AudioSource>();
            Debug.Log("button::" + button + "audioSource::" + audioSource);
            button.onClick.AddListener(PlayTapSound);
        }
        
    }

    private void PlayTapSound()
    {
        if (tapSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(tapSound);
            Debug.Log("Played "+tapSound);
        }
    }
}
