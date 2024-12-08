using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public AudioClip backgroundMusic;
    public Slider volumeSlider;

    private void Start()
    {
        // Load the saved volume from PlayerPrefs, default to 0.5 if no value exists
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);

        // Apply the saved volume
        audioSource.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // Set up the audio source
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }

    private void Update()
    {
        // Update the audio volume based on the slider value
        audioSource.volume = volumeSlider.value;

        // Save the current volume to PlayerPrefs
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
}
