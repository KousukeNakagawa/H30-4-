using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMove : MonoBehaviour
{
    public enum DroneDirection
    {
        Advance,  //前
        Recession  //後ろ
    }

    private enum DroneState
    {
        FlyUp,  //始動
        Search,  //探索
        Return,  //終了
    }

    [HideInInspector] public DroneDirection droneDirection = DroneDirection.Advance;
    private DroneState droneState = DroneState.FlyUp;
    [Tooltip("前(X)、横(Z)への移動速度")] public float moveSpeed = 10.0f;
    [Tooltip("前(X)、横(Z)への移動範囲")] public Vector2 moveArea = new Vector2(35.0f, 70.0f);
    [Tooltip("Xに進む分割数")] public int XFraction = 5;
    [Tooltip("ロボットへ戻るときの移動速度")] public float returnSpeed = 20.0f;
    [Tooltip("見つけた時の追尾時間")]public float searchFollowTime = 10.0f;
    [HideInInspector] public Transform followObj = null;
    [Tooltip("ドローン出現時、上昇する速度"),SerializeField] private float flyUpSpeed = 1.0f;

    private Vector2 primaryPos;  //初期位置(x,z)
    private Vector2 onceTargetPos;  //到達したらターンする
    private Vector2 initialPos;
    private float goalPos;
    private Vector3 velocity;
    private int sagittalDir;
    private int LRdir = 1;
    public Collider m_collider;
    private bool isGo; //前に進んでいるか（X）
    private float followCount;
    [SerializeField,Tooltip("自分のSnipeBulletHitAction")] private SnipeBulletHitAction snipeBullet;

    // Use this for initialization
    void Start()
    {
        followCount = searchFollowTime;
        if (droneDirection == DroneDirection.Recession) flyUpSpeed *= 0.75f;
        primaryPos = new Vector2(transform.position.x, transform.position.z);
        initialPos = new Vector2(transform.position.x, transform.position.z + moveArea.y / 2);
        Vector3 dir = new Vector3(0, 0, initialPos.y - primaryPos.y);
        velocity = dir.normalized;
        velocity.y = flyUpSpeed;
        if (droneDirection == DroneDirection.Advance) sagittalDir = 1;
        else sagittalDir = -1;
        goalPos = transform.position.x + sagittalDir * moveArea.x;
    }

    // Update is called once per frame
    void Update()
    {
        switch (droneState)
        {
            case DroneState.FlyUp:
                if ((initialPos - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude <= 0.1f)
                {
                    velocity = new Vector3(sagittalDir, 0, 1).normalized;
                    droneState++;
                    isGo = true;
                    onceTargetPos = new Vector2(transform.position.x + sagittalDir * (moveArea.x / XFraction),
                        transform.position.z);
                    m_collider.enabled = true;
                }
                else
                {
                    transform.Translate(velocity * moveSpeed * Time.deltaTime);
                }
                break;
            case DroneState.Search:
                if (followObj != null)
                {
                    if (followCount < 0)
                    {
                        droneState++;
                        velocity = Vector3.forward;
                        m_collider.enabled = false;
                    }
                    else
                    {
                        followCount -= Time.deltaTime;
                        Vector2 followDir = new Vector2(followObj.position.x - transform.position.x,
                            followObj.position.z - transform.position.z).normalized;
                        transform.Translate(new Vector3(followDir.x, 0, followDir.y) * Time.deltaTime * moveSpeed);
                        return;
                    }
                }
                if (Mathf.Abs(Mathf.Abs(goalPos) - Mathf.Abs(transform.position.x)) <= 0.1f)
                {
                    droneState++;
                    velocity = Vector3.forward;
                    m_collider.enabled = false;
                    return;
                }

                if ((onceTargetPos - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude <= 0.1f)
                {
                    isGo = !isGo;
                    if (!isGo)  //次進む方向は左右
                    {
                        LRdir = -LRdir;
                        onceTargetPos = new Vector2(transform.position.x, transform.position.z + moveArea.y * LRdir);
                    }
                    else  //次進む方向は前
                    {
                        onceTargetPos = new Vector2(transform.position.x + sagittalDir * moveArea.x / XFraction,
                            transform.position.z);
                    }
                }
                else
                {
                    if (isGo) transform.Translate(velocity.x * moveSpeed * Time.deltaTime, 0, 0);
                    else transform.Translate(0, 0, velocity.z * LRdir * moveSpeed * Time.deltaTime);
                }
                break;
            case DroneState.Return:
                transform.Translate(velocity * returnSpeed * Time.deltaTime, Space.Self);
                if ((new Vector2(BigEnemyScripts.mTransform.position.x, BigEnemyScripts.mTransform.position.z) -
                    new Vector2(transform.position.x, transform.position.z)).sqrMagnitude <= 0.1f)
                {
                    transform.parent = BigEnemyScripts.mTransform;
                    velocity = Vector3.down / 4;
                    Destroy(gameObject, 0.75f);
                }
                else
                {
                    Vector3 dir = BigEnemyScripts.mTransform.position - transform.position;
                    transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                }
                break;
        }
    }

    void OnDestroy()
    {
        BigEnemyScripts.droneCreate.RemoveDrone(gameObject);
        BigEnemyScripts.shootingPhaseMove.makebyRobot.Remove(gameObject);
    }
}
