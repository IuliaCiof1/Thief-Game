using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectArea : MonoBehaviour
{
    [SerializeField] AudioClip collectSound;

   public void PlayCollectSoud()
    {
        SoundFXManager.instance.PlaySoundFXClip(collectSound, transform, 1f);
    }
}
