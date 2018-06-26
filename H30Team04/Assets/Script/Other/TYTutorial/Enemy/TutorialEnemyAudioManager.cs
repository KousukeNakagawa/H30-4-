//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyAudioManager : MonoBehaviour {
    [Header("BigEnemyAudioTypeと順番を合わせてください")]
    [Tooltip("巨大ロボが再生する音声"), SerializeField]
    private List<AudioClip> audios;
    [Space(20)]
    [Tooltip("空のオーディオソース"), SerializeField]
    private GameObject emptyAudio;
    [Tooltip("巨大ロボについているオーディオソース"), SerializeField] private AudioSource m_audio;

    public void CreateSound(BigEnemyAudioType type, Transform trans)
    {
        CreateSound(audios[(int)type], trans.position);
    }

    public void CreateSound(AudioClip clip, Vector3 pos)
    {
        GameObject audio = Instantiate(emptyAudio, pos, Quaternion.identity);
        audio.GetComponent<AudioSource>().clip = clip;
        audio.GetComponent<AudioSource>().Play();
    }

    public void Play(BigEnemyAudioType type, Transform target = null)
    {
        Transform t = target ?? TutorialEnemyScripts.mTransform;
        switch (type)
        {
            case BigEnemyAudioType.Explosion:
                AudioSource.PlayClipAtPoint(audios[(int)type], t.position, 3.0f);
                break;
            default:
                m_audio.clip = audios[(int)type];
                m_audio.Play();
                break;
        }
    }
}
