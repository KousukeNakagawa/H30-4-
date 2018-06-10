using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BigEnemyAudioType
{
    Step,
    Explosion,
}

public class BigEnemyAudioManager : MonoBehaviour {

    [SerializeField] private List<AudioClip> audios;
    [SerializeField] private GameObject emptyAudio;

    private bool previousIsStep;

	// Use this for initialization
	void Awake () {
    }
	
	// Update is called once per frame
	void Update () {
		if (BigEnemyScripts.bigEnemyAnimatorManager.isStep && !previousIsStep)
        {
            Play(BigEnemyAudioType.Step);
        }
        previousIsStep = BigEnemyScripts.bigEnemyAnimatorManager.isStep;
	}

    public void CreateSound(BigEnemyAudioType type,Transform trans)
    {
        CreateSound(audios[(int)type],trans.position);
    }

    public void CreateSound(AudioClip clip,Vector3 pos)
    {
        GameObject audio = Instantiate(emptyAudio, pos, Quaternion.identity);
        audio.GetComponent<AudioSource>().clip = clip;
        audio.GetComponent<AudioSource>().Play();
    }

    public void Play(BigEnemyAudioType type,Transform target = null)
    {
        Transform t = target ?? BigEnemyScripts.mTransform;
        AudioSource.PlayClipAtPoint(audios[(int)type], t.position);
    }
}
