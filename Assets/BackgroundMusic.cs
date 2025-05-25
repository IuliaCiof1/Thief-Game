using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] AudioClip mainMenuMusic;
    [Range(0f,1f)]
    [SerializeField] float volume;
    AudioSource audioSource;

    private void Start()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
            Debug.Log("No AudioSource component found on " + gameObject.name);

        PlayMusicClip(mainMenuMusic, transform, volume);
    }



    public void PlayMusicClip(AudioClip audioclip, Transform spawnTransform, float volume)
    {

        audioSource.loop = true;
        audioSource.clip = audioclip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLenth = audioSource.clip.length;

    }
}
