using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAudioPlay : MonoBehaviour {

    public enum AudioType
    {
        Search,
    }

    [SerializeField] private AudioClip[] audios;
    private AudioSource m_audio;

    // Use this for initialization
    void Start () {
        m_audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play(AudioType type,bool isLoop)
    {
        m_audio.Stop();
        m_audio.clip = audios[(int)type];
        m_audio.loop = isLoop;
        m_audio.Play();
    }

    public void Stop()
    {
        m_audio.Stop();
    }
}
