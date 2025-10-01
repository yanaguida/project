using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour
{
    public AudioSource audioSource;   // 再生用 AudioSource
    public AudioClip smileClip;
    public AudioClip sadClip;
    public AudioClip winkClip;
    

    public void Play(string music)
    {
        if (audioSource == null) return;

        switch (music)
        {
            case "Smile":
                audioSource.clip = smileClip;
                break;
            case "Sad":
                audioSource.clip = sadClip;
                break;
            case "Wink":
                audioSource.clip = winkClip;
                break;
            default:
                audioSource.clip = null;
                break;
        }

        if (audioSource.clip != null)
            audioSource.Play();
    }

    public void Stop()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }

    public IEnumerator PlayForSeconds(float seconds, string music)
    {
        Play(music);
        yield return new WaitForSeconds(seconds);
        Stop();
    }
}

