using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoSingleton<SoundMgr>
{
    [SerializeField] List<SoundInfo> soundInfos = new List<SoundInfo>();
    List<AudioSource> audioSources = new List<AudioSource>();
    public AudioSource[] BGMSources;

    public void PlayBGM(int idx)
    {
        for(int  i=0;i< BGMSources.Length; i++)
        {
            BGMSources[i].Stop();
        }
        BGMSources[idx].Play();
    }
    public void StopBGM()
    {
        for (int i = 0; i < BGMSources.Length; i++)
            BGMSources[i].Stop();
    }

    void Start()
    {
        
        for (int i = 0; i < 15; i++)
        {
            //AudioSource 컴포넌트 생성하기
            audioSources.Add(gameObject.AddComponent<AudioSource>());

            audioSources[i].loop = false;
            audioSources[i].playOnAwake = false;
        }
    }

    //사운드 실행하기
    public void PlaySound(string sType)
    {
        SoundInfo sInfo = GetSoundInfo(sType);
        if (sInfo == null)
            return;

        AudioSource aSource = GetAudioSource();

        aSource.clip = sInfo.audioClip;
        aSource.volume = sInfo.volume;

        aSource.Play();
    }

    //사운드 데이터 가져오기
    SoundInfo GetSoundInfo(string sType)
    {
        for (int i = 0; i < soundInfos.Count; i++)
        {
            if (soundInfos[i].sfxType.Equals(sType))
            {
                return soundInfos[i];
            }
        }
        return null;
    }

    //사용하지 않은 AudioSource 컴포넌트 가져오기
    AudioSource GetAudioSource()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].isPlaying)
                continue;

            return audioSources[i];
        }

        return audioSources[0]; //모든 AudioSource 컴포넌트 사용중이면 [0]번째 AudioSource 반환
    }

#if UNITY_EDITOR
    public void Edit()
    {
        
        AudioClip[] clips = Resources.LoadAll<AudioClip>($"Sounds");
        
        for (int i = 0; i < clips.Length; i++)
        {
            Debug.Log(clips[i].name);
            SoundInfo info = GetSoundInfo(clips[i].name);
            if (info == null)
            {
                info = new SoundInfo();
                info.sfxType = clips[i].name;
                info.volume = 1;
                info.audioClip = clips[i];
                soundInfos.Add(info);
            }

            //info.audioClip = Resources.Load<AudioClip>($"sounds/{info.sfxType}");
            //if(info.audioClips == null || info.audioClips.Length <= 0)
            //{
            //    Debug.Log($"Need Audio {info.sfxType}");
            //}
        }
    }
#endif
}

//public enum SFXType :int//사운드 종류
//{
//    click,
//    monsterDestroy,
//    reward,
//    error,
//    exp,
//    upgrade,
//    setup,
//    count
//}


[System.Serializable]
public class SoundInfo //사운드 관련 데이터
{
    public string sfxType;
    public AudioClip audioClip;
    public float volume;

    //public AudioClip GetAudioClip()
    //{
    //    if (audioClips.Length <= 1)
    //        return audioClips[0];
    //    return audioClips[Random.Range(0, audioClips.Length)];
    //}
}