using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnsyutuCamera : MonoBehaviour {

    public Transform[] changePos;
    public Transform[] targetPos; //タワー、ｐバック、ビーコン、誘導、射影機
    public Transform enemyPos;

    private int nowtargetPos = 0;
    private int nowInt = 0;
    private bool isChange = false;

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

    public Animator beaconAnim;

    private bool enemyGo = false;


	// Use this for initialization
	void Start () {
        m_audio = GetComponent<AudioSource>();

        UnlockManager.AllSet(true);
        UnlockManager.Lock(UnlockState.snipe);

        StartText.TextStart(nowInt);
        setumeibunkatu = changePos[0].position.x - enemyPos.position.x;

    }
	
	// Update is called once per frame
	void Update () {
        //if(nowtargetPos < changePos.Length&& !isChange && enemyPos.position.x > changePos[nowtargetPos].position.x)
        //      {
        //          startPos = transform.position;
        //          startRotate = transform.rotation;
        //          startTime = Time.time;
        //          isChange = true;
        //          kyori = Vector3.Distance(startPos, targetPos[nowtargetPos].position);

        //          switch (nowtargetPos)
        //          {
        //              case 1:
        //                  startRippel.StartEffect();
        //                  startWepon.ChengeLaser();
        //                  break;
        //              case 2:
        //                  startRippel.EndEffect();
        //                  startWepon.ChengeLaser();
        //                  m_audio.PlayOneShot(shotSound);
        //                  break;
        //          }
        //      }

        if (Fade.IsFadeEnd() && Fade.IsFadeOutOrIn())
        {
            ScecnManager.SceneChange("GamePlay");
        }


        if (Input.GetButtonDown("Shutter"))
        {
            if(nowInt < 2)
            {
                nowInt++;
                StartText.TextStart(nowInt);
            }
            else if(nowtargetPos < changePos.Length && !isChange)
            {
                startPos = transform.position;
                startRotate = transform.rotation;
                startTime = Time.time;
                isChange = true;
                kyori = Vector3.Distance(startPos, targetPos[nowtargetPos].position);

                switch (nowtargetPos)
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
                    case 3:
                        enemyPos.position = changePos[nowtargetPos].position;
                        enemyGo = true;
                        break;
                }
            }
            else if(nowtargetPos >= changePos.Length)
            {
                Fade.FadeOut();
            }

        }

        if (isChange) //カメラ移動中
        {
            float nowTime = (Time.time - startTime) * lerpSpeed;
            //float step = Time.deltaTime * lerpTime;
            float nowPos = nowTime / kyori;
            transform.position = Vector3.Lerp(startPos, targetPos[nowtargetPos].position, nowPos);
            //transform.eulerAngles = Vector3.Lerp(startRotate, targetPos[nowInt].eulerAngles, nowPos);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetPos[nowInt].rotation, nowPos);
            //float angle = Mathf.LerpAngle(minAngle, maxAngle, Time.time);
            //transform.eulerAngles = new Vector3(0, angle, 0);
            transform.rotation = Quaternion.Lerp(startRotate, targetPos[nowtargetPos].rotation, nowPos);


            if (nowPos >= 1)
            {
                if(nowtargetPos == 2)
                {
                    beaconAnim.Play("beaconAnim");
                    m_audio.PlayOneShot(beaconSound);
                }
                nowtargetPos++;
                isChange = false;
                StartText.TextStart(nowtargetPos + 2);
            }
        }

        if(Vector2.SqrMagnitude(startbeacon.transform.position.ToTopView()-enemyPos.position.ToTopView()) < 25)
        {
            startbeacon.SetActive(false);
        }


        if (nowtargetPos < 3 && enemyPos.position.x >= changePos[0].position.x)
        {
            enemyPos.position = changePos[0].position;
        }
        else if (!enemyGo && enemyPos.position.x >= changePos[0].position.x)
        {
            enemyPos.position = changePos[3].position;
        }

        //if(nowtargetPos == 0 && (setumeibunkatu /3) * 2 > changePos[0].position.x - enemyPos.position.x)
        //{
        //    StartText.TextStart(6);
        //    if ((setumeibunkatu / 3) > changePos[0].position.x - enemyPos.position.x)
        //    {
        //        StartText.TextStart(7);
        //    }
        //}
    }
}
