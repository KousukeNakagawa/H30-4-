using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMove2 : MonoBehaviour
{
    private enum StateType
    {
        Rise, Rotation, Fall,
    }

    public float riseCount = 2.0f;  //上昇する秒数
    public float rotationCount = 5.0f;  //回転する秒数
    public float riseSpeed = 20.0f;  //通常の速度速度
    public float TransSpeed = 10.0f;  //回転する際の移動速度

    private Rigidbody rigid;
    private Vector3 targetPos;
    private StateType state;
    public float rate = 0f;
    private Quaternion primary;
    private float riseTime;

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
    {
        switch (state)
        {
            case StateType.Rise:
                if (riseTime < Time.time)
                {
                    state++;
                }
                break;
            case StateType.Rotation:
                rate = rate + Time.deltaTime * (1 / rotationCount);
                Ray ray = new Ray(transform.position + transform.forward, transform.forward);
                List<RaycastHit> hits = new List<RaycastHit>(Physics.RaycastAll(ray));
                if (hits.FindAll(f => (f.point - targetPos).magnitude <= 0.7f).Count != 0)
                {
                    state++;
                    rigid.velocity = Vector3.zero;
                    rigid.useGravity = false;
                    rigid.AddForce((targetPos - transform.position).normalized * riseSpeed,
                        ForceMode.VelocityChange);
                }
                break;
            case StateType.Fall:
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case StateType.Rise:
                rigid.AddForce(transform.forward * riseSpeed);
                break;
            case StateType.Rotation:
                rigid.AddForce(-Physics.gravity * 0.5f);
                transform.Translate(0, 0, Time.deltaTime * TransSpeed, Space.Self);
                Vector3 dir = Vector3.Slerp(primary.eulerAngles, Quaternion.LookRotation(targetPos - transform.position).eulerAngles.GetUnityVector3(), rate);
                transform.rotation = Quaternion.Euler(dir);
                break;
            case StateType.Fall:
                rigid.AddForce(transform.forward * riseSpeed);
                break;
        }
    }
}
