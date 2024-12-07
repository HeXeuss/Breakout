using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public AudioClip backgroundMusic;
    public static float volume = 0.5f;
    public Slider volumeSlider;
    private void Start()
    {
        volumeSlider.value = volume;
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }

    private void Update()
    {
        volume = volumeSlider.value;
        audioSource.volume = volume;
    }
    
}
