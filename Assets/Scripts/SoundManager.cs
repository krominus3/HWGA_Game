using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip coinSound; 
    [SerializeField] private AudioClip landingSound;
    [SerializeField] private AudioClip getHitSound;
    [SerializeField] private AudioClip enemyGetHitSound;
    [SerializeField] private AudioClip preJumpSound;
    [SerializeField] private AudioClip shotSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlaySound(AudioClip audioClip){
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayLandingSound(){
        audioSource.PlayOneShot(landingSound);
    }
    public void PlayCoinSound(){
        audioSource.PlayOneShot(coinSound);
    }
    public void PlayGetHitSound(){
        audioSource.PlayOneShot(getHitSound);
    }
    public void PlayEnemyGetHitSound(){
        audioSource.PlayOneShot(enemyGetHitSound);
    }
    public void PlayPreJumpSound(){
        audioSource.PlayOneShot(preJumpSound);
    }
    public void PlayShotSound(){
        audioSource.PlayOneShot(shotSound);
    }
}
