using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] List<AudioSource> audioSources = new List<AudioSource>();
    [SerializeField] int poolingCount = 3;
    [SerializeField] AudioClip[] clips;
    [SerializeField] float volume;
    void Start()
    {
        int asCount = audioSources.Count;
        for (int i = 0; i < poolingCount; i++)
        {
            if(i >= asCount)
            {

                //AudioSource 컴포넌트 생성하기
                audioSources.Add(gameObject.AddComponent<AudioSource>());

                audioSources[i].loop = false;
                audioSources[i].playOnAwake = false;
                audioSources[i].volume = volume;
            }
        }
    }

    //사운드 실행하기
    public void PlaySound()
    {
        AudioSource aSource = GetAudioSource();
        aSource.clip = GetAudioClip();
        aSource.Play();
    }
    AudioClip GetAudioClip()
    {
        if (clips.Length <= 1)
            return clips[0];

        return clips[Random.Range(0, clips.Length)];
    }

    //사운드 실행하기
    public void StopSound()
    {
        for(int i =0;i< audioSources.Count; i++)
        {
            audioSources[i].Stop();
        }
    }

    int idx = 0;
    //사용하지 않은 AudioSource 컴포넌트 가져오기
    AudioSource GetAudioSource()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].isPlaying)
                continue;

            return audioSources[i];
        }
        idx++;
        if (idx >= audioSources.Count)
            idx = 0;

        return audioSources[idx]; //모든 AudioSource 컴포넌트 사용중이면 [0]번째 AudioSource 반환
    }

}
