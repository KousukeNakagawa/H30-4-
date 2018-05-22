using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMove : MonoBehaviour
{

    [Tooltip("飛ぶスピード")]public float speed = 20.0f;
    [Tooltip("上昇する時間")]public float riseTime = 2.0f;
    [Range(0, 3),Tooltip("回転を行う秒数")] public float rotationTime = 2.0f;
    private float timeCount;  //Rise→Rotationへ移行するためのカウンター

    private Quaternion primaryRotation;  //最初の回転情報
    private float rotationCount;  //Slerpで使うカウンター
    [Tooltip("自分のRigidBody")]public Rigidbody rigid;  //自身のRigidBody

    private enum MissileStateType
    {
        Rise,  //上昇中
        LookRotation,  //回転中
        Fall,  //落下中
    }

    private MissileStateType stateType = MissileStateType.Rise;

    // Use this for initialization
    void Start()
    {
        timeCount = Time.time + riseTime;
        primaryRotation = transform.rotation;
    }

    void Update()
    {  //移動と回転関連をこちらで行う
        Vector3 target = BigEnemyScripts.searchObject.targetPos;
        switch (stateType)
        {
            case MissileStateType.Rise:
                if (Time.time > timeCount)
                {
                    stateType++;
                    timeCount = Time.time + rotationTime;
                }
                break;
            case MissileStateType.LookRotation:
                rotationCount += Mathf.Clamp01(Time.deltaTime * (1f / rotationTime));
                transform.rotation = Quaternion.Slerp(primaryRotation, Quaternion.LookRotation(target - transform.position), rotationCount);
                break;
            case MissileStateType.Fall:
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {  //RigidBody関連をこちらで処理を行う
        switch (stateType)
        {
            case MissileStateType.Rise:
                rigid.AddForce(transform.TransformDirection(Vector3.forward) * speed);
                break;
            case MissileStateType.LookRotation:
                if (rotationCount >= 1)
                {
                    stateType = MissileStateType.Fall;
                    float vel = rigid.velocity.magnitude;
                    rigid.useGravity = false;
                    rigid.velocity = Vector3.zero;
                    rigid.AddForce(transform.forward * vel, ForceMode.VelocityChange);
                }
                break;
            case MissileStateType.Fall:
                rigid.AddForce(transform.forward * speed);
                break;
        }
    }
}
