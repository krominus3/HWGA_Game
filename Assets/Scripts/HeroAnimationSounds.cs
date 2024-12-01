using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> walkAudioList;
    private void PlayAudio(List<AudioClip> audioClips)
    {
        int index = Random.Range(0, audioClips.Count);
        audioSource.PlayOneShot(audioClips[index]);
    }

    private void PlayAudio(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayWalk()
    {
        PlayAudio(walkAudioList);
    }
}
