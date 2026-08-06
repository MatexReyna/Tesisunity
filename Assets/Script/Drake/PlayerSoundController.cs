using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip sonidoCorrer;
    public AudioClip sonidoSaltar;
    public AudioClip sonidoDash;
    public AudioClip sonidoValla;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayCorrer()
{
    if (!audioSource.isPlaying)
    {
        audioSource.clip = sonidoCorrer;
        audioSource.loop = true;
        audioSource.Play();
    }
}

public void StopCorrer()
{
    if (audioSource.clip == sonidoCorrer)
    {
        audioSource.Stop();
        audioSource.loop = false;
    }
}

public void playSaltar()
{
    Debug.Log("SALTO");

    audioSource.Stop();
    audioSource.clip = sonidoSaltar;
    audioSource.loop = false;
    audioSource.Play();
}

public void PlayDash()
{
    audioSource.Stop();
    audioSource.clip = sonidoDash;
    audioSource.loop = false;
    audioSource.Play();
}

public void PlayValla()
{
    audioSource.Stop();
    audioSource.PlayOneShot(sonidoValla);
}
}