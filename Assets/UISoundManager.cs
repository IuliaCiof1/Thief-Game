using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

public class UISoundManager : MonoBehaviour
{
    [SerializeField] AudioClip buttonSoundClip;
    AudioSource audioSource;

    //[Range(0f, 1f)]
   // [SerializeField] float volume;

    [SerializeField] bool addSoundToUI;

    private void Start()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
            Debug.Log("No AudioSource component found on " + gameObject.name);

        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (Button button in buttons)
        {
            button.onClick.RemoveListener(() => audioSource.PlayOneShot(buttonSoundClip));
            button.onClick.AddListener(() => audioSource.PlayOneShot(buttonSoundClip));

            //UnityEventTools.AddPersistentListener(button.onClick, () => audioSource.PlayOneShot(buttonSoundClip));
        }

    }

    //private void OnValidate()
    //{
    //    if (!TryGetComponent<AudioSource>(out audioSource))
    //        Debug.Log("No AudioSource component found on " + gameObject.name);

    //    Button[] buttons = FindObjectsOfType<Button>(true);
    //    foreach (Button button in buttons)
    //    {
    //        //button.onClick.RemoveListener(() => audioSource.PlayOneShot(buttonSoundClip, volume));
    //        //button.onClick.AddListener(() => audioSource.PlayOneShot(buttonSoundClip, volume));

    //        UnityEventTools.RemovePersistentListener(button.onClick, () => audioSource.PlayOneShot(buttonSoundClip));
    //        UnityEventTools.AddPersistentListener(button.onClick, () => audioSource.PlayOneShot(buttonSoundClip));
    //    }
    //}


    //public void PlaySoundClip(AudioClip audioclip, float volume)
    //{

    //    audioSource.loop = true;
    //    audioSource.clip = audioclip;
    //    audioSource.volume = volume;
    //    audioSource.Play();
    //    float clipLenth = audioSource.clip.length;

    //}
}
