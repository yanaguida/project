using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface Ifunc
{
    IEnumerator Action(float start, float time, float data);
    PartType CorrespondPart();
}

public class Audio : MonoBehaviour, Ifunc
{
    [SerializeField] private PartType parttype;
    [SerializeField] private AudioSource audioSource;   // 再生用 AudioSource
    [SerializeField] private AudioClip happyClip;
    [SerializeField] private AudioClip sadClip;
    [SerializeField] private AudioClip angryClip;
    [SerializeField] private AudioClip enjoyClip;
    private List<AudioClip> AudioList = new List<AudioClip>();

    IEnumerator Start()
    {
        AudioList.Add(happyClip);
        AudioList.Add(sadClip);
        AudioList.Add(angryClip);
        AudioList.Add(enjoyClip);
        audioSource.mute = true;
        // 無音で1フレームだけ再生してデコードしておく
        foreach (var audio in AudioList)
        {
            audioSource.clip = audio;
            audioSource.Play();
            yield return null;
            audioSource.Stop();
        }
        audioSource.mute = false;
    }

    public PartType CorrespondPart() => parttype;

    private float pausedTime = 0f;     // 停止した位置を記録
    private float currentMusic = -1f;  // 現在の曲名を記録

    public IEnumerator Action(float start, float seconds, float music)
    {
        yield return new WaitForSeconds(start);
        Play(music);
        yield return new WaitForSeconds(seconds);
        Stop();
    }

    private void Play(float music)
    {
        if (audioSource == null) return;

        if (currentMusic != music)
        {
            pausedTime = 0f;
            currentMusic = music;
        }

        switch (music)
        {
            case 0:
                audioSource.clip = happyClip;
                break;
            case 1:
                audioSource.clip = sadClip;
                break;
            case 2:
                audioSource.clip = angryClip;
                break;
            case 3:
                audioSource.clip = enjoyClip;
                break;
            default:
                audioSource.clip = null;
                break;
        }

        if (audioSource.clip != null)
        {
            audioSource.time = pausedTime;
            audioSource.Play();
        }
    }

    public void Pause()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            pausedTime = audioSource.time; // 現在の再生時間を記録
            audioSource.Pause();           // 一時停止
        }
    }

    private void Stop()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            pausedTime = 0f;
            audioSource.Stop();
        }
    }
}