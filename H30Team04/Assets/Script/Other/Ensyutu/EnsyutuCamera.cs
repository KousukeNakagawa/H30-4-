using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnsyutuCamera : MonoBehaviour {

    public Transform[] chengePos;
    public Transform[] targetPos;
    public Transform enemyPos;

    private int nowInt = 0;
    private bool isChenge = false;

    private Vector3 startPos;
    private Quaternion startRotate;
    private float startTime;
    private float kyori;

    [SerializeField]private float lerpSpeed = 10.0f;

    public GameObject startbeacon;
    public StartWepon startWepon;
    public StartRippel startRippel;

    private float setumeibunkatu = 0.0f;

    public AudioClip beaconSound, shotSound;
    private AudioSource m_audio;


	// Use this for initialization
	void Start () {
        m_audio = GetComponent<AudioSource>();

        UnlockManager.AllSet(true);
        UnlockManager.Lock(UnlockState.snipe);

        StartText.TextStart(nowInt);
        setumeibunkatu = chengePos[0].position.x - enemyPos.position.x;

    }
	
	// Update is called once per frame
	void Update () {
		if(nowInt < chengePos.Length&& !isChenge && enemyPos.position.x > chengePos[nowInt].position.x)
        {
            startPos = transform.position;
            startRotate = transform.rotation;
            startTime = Time.time;
            isChenge = true;
            kyori = Vector3.Distance(startPos, targetPos[nowInt].position);

            switch (nowInt)
            {
                case 1:
                    startRippel.StartEffect();
                    startWepon.ChengeLaser();
                    break;
                case 2:
                    startRippel.EndEffect();
                    startWepon.ChengeLaser();
                    m_audio.PlayOneShot(shotSound);
                    break;
            }
        }

        if (isChenge)
        {
            float nowTime = (Time.time - startTime) * lerpSpeed;
            //float step = Time.deltaTime * lerpTime;
            float nowPos = nowTime / kyori;
            transform.position = Vector3.Lerp(startPos, targetPos[nowInt].position, nowPos);
            //transform.eulerAngles = Vector3.Lerp(startRotate, targetPos[nowInt].eulerAngles, nowPos);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetPos[nowInt].rotation, nowPos);
            //float angle = Mathf.LerpAngle(minAngle, maxAngle, Time.time);
            //transform.eulerAngles = new Vector3(0, angle, 0);
            transform.rotation = Quaternion.Lerp(startRotate, targetPos[nowInt].rotation, nowPos);


            if (nowPos >= 1)
            {
                if(nowInt == 2)
                {
                    m_audio.PlayOneShot(beaconSound);
                }
                nowInt++;
                isChenge = false;
                StartText.TextStart(nowInt);
            }
        }

        if(Vector2.SqrMagnitude(startbeacon.transform.position.ToTopView()-enemyPos.position.ToTopView()) < 25)
        {
            startbeacon.SetActive(false);
        }

        if(nowInt == 0 && (setumeibunkatu /3) * 2 > chengePos[0].position.x - enemyPos.position.x)
        {
            StartText.TextStart(6);
            if ((setumeibunkatu / 3) > chengePos[0].position.x - enemyPos.position.x)
            {
                StartText.TextStart(7);
            }
        }
	}
}
