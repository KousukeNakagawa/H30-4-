using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMove2 : MonoBehaviour
{
    private enum StateType
    {
        Rise,  //上昇中
        Rotation,  //回転中
        Fall,  //落下中
    }

    [Tooltip("上昇する秒数")]public float riseCount = 2.0f; 
    [Tooltip("回転する秒数")]public float rotationCount = 5.0f;
    [Tooltip("通常の上昇速度")]public float riseSpeed = 20.0f;
    [Tooltip("回転する時の移動速度")]public float TransSpeed = 10.0f;

    private Rigidbody rigid;  //自身のRigidBody
    private Vector3 targetPos;  //目標座標
    private StateType state;  //ミサイルの状態
    private float rate = 0f;  //Slerpを使用する時のカウント
    private Quaternion primary;  //一番最初の角度
    private float riseTime;  //上昇する時のカウント

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        primary = transform.rotation;
        riseTime = Time.time + riseCount;
        targetPos = BigEnemyScripts.searchObject.targetPos;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * 5);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 5);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.right * 5);
    }

    void Update()
    {  //状態の更新やカウンター処理などはこっちで行う
        switch (state)
        {
            case StateType.Rise:  //上昇
                if (riseTime < Time.time)
                {
                    state++; //状態更新
                }
                break;
            case StateType.Rotation:  //回転
                rate = rate + Time.deltaTime * (1 / rotationCount);
                Ray ray = new Ray(transform.position + transform.forward, transform.forward);
                List<RaycastHit> hits = new List<RaycastHit>(Physics.RaycastAll(ray));
                if (hits.FindAll(f => (f.point - targetPos).magnitude <= 0.7f).Count != 0)
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
            case StateType.Rise:  //上昇
                rigid.AddForce(transform.forward * riseSpeed);
                break;
            case StateType.Rotation:  //回転
                rigid.AddForce(-Physics.gravity * 0.5f);
                transform.Translate(0, 0, Time.deltaTime * TransSpeed, Space.Self);
                Vector3 dir = Vector3.Slerp(primary.eulerAngles, Quaternion.LookRotation(targetPos - transform.position).eulerAngles.GetUnityVector3(), rate);
                transform.rotation = Quaternion.Euler(dir);
                break;
            case StateType.Fall:  //落下
                rigid.AddForce(transform.forward * riseSpeed);
                break;
        }
    }
}
