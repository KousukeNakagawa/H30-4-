using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissileMove2 : MonoBehaviour
{
    private enum StateType
    {
        Initial,  //初期動作
        Rise,  //上昇中
        Rotation,  //回転中
        Fall,  //落下中
    }

    [Tooltip("上昇する秒数")] public float riseCount = 2.0f;
    [Tooltip("回転する秒数")] public float rotationCount = 5.0f;
    [Tooltip("通常の上昇速度")] public float riseSpeed = 20.0f;
    [Tooltip("回転する時の移動速度")] public float TransSpeed = 10.0f;
    [Tooltip("初期動作の時間")] public float initialCount = 2.0f;

    private Rigidbody rigid;  //自身のRigidBody
    [HideInInspector] public Vector3 targetPos = Vector3.zero;  //目標座標
    private StateType state = StateType.Initial;  //ミサイルの状態
    private float rate = 0f;  //Slerpを使用する時のカウント
    private Quaternion primary;  //一番最初の角度
    private float riseTime;  //上昇する時のカウント
    private float initialTime;  //初期動作のカウント
    [HideInInspector] public int velocity = 1;
    [SerializeField,Range(0.0f,1.0f)] private float initialRate = 0.05f;
    [Tooltip("ファイアの音声"),SerializeField] private AudioSource fire_audio;

    void Awake()
    {
        initialTime = Time.time + initialCount;
        targetPos = (targetPos == Vector3.zero) ? BigEnemyScripts.searchObject.targetPos : targetPos;
        BigEnemyScripts.shootingPhaseMove.makebyRobot.Add(gameObject);
        primary = transform.rotation;
    }

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(transform.forward * velocity * riseSpeed * 0.5f, ForceMode.Impulse);
        GetComponent<AudioSource>().Play();
    }
    void Update()
    {  //状態の更新やカウンター処理などはこっちで行う
        if (Time.timeScale == 0) return;
        switch (state)
        {
            case StateType.Initial:
                if (initialTime < Time.time)
                {
                    state++;
                    riseTime = Time.time + riseCount;
                    fire_audio.Play();
                }
                break;
            case StateType.Rise:  //上昇
                if (riseTime < Time.time)
                {
                    state++; //状態更新
                    primary = transform.rotation;
                }
                break;
            case StateType.Rotation:  //回転
                rate = rate + Time.deltaTime * (1 / rotationCount);
                if (rate >= 1.0f)
                {
                    state++;  //状態更新
                    rigid.velocity = Vector3.zero;
                    rigid.useGravity = false;
                    rigid.AddForce((targetPos - transform.position).normalized * riseSpeed,
                        ForceMode.VelocityChange);
                }
                break;
            case StateType.Fall:  //落下
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {  //移動、回転系統の処理はこっちで行う
        switch (state)
        {
            case StateType.Initial:
                rigid.AddForce(transform.forward * velocity * riseSpeed * 0.5f,ForceMode.Acceleration);
                rigid.AddForce(-Physics.gravity);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.Euler(-90.0f, transform.eulerAngles.y, transform.eulerAngles.z), initialRate);
                break;
            case StateType.Rise:  //上昇
                rigid.AddForce(transform.forward * velocity * riseSpeed);
                break;
            case StateType.Rotation:  //回転
                rigid.AddForce(-Physics.gravity * 0.5f);
                transform.Translate(0, 0, Time.deltaTime * TransSpeed, Space.Self);
                Vector3 dir = Vector3.Slerp(primary.eulerAngles, Quaternion.LookRotation(targetPos - transform.position).eulerAngles.GetUnityVector3(), rate);
                transform.rotation = Quaternion.Euler(dir);
                break;
            case StateType.Fall:  //落下
                rigid.AddForce(transform.forward * riseSpeed);
                transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
                break;
        }
    }

    void OnDestroy()
    {
        BigEnemyScripts.shootingPhaseMove.makebyRobot.Remove(gameObject);
    }
}
