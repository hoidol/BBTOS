using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundBtn : MonoBehaviour
{

    [SerializeField] AudioClip clip;
    [Range(0,1)]
    [SerializeField] float volume;
    [SerializeField] bool playOnAwake;
    [SerializeField] bool loop;
    AudioSource audio;

    public  void Awake()
    {
        
        audio = gameObject.AddComponent<AudioSource>();
        audio.playOnAwake = playOnAwake;
        audio.loop = loop;
        audio.volume = volume;
    }

    public  void OnClicked()
    {
        audio.Play();
    }


}
