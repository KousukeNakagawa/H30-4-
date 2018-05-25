using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMove2 : MonoBehaviour
{
    public enum DroneDirection
    {
        Advance,  //前
        Recession  //後ろ
    }

    private enum DroneState
    {
        Start,  //出現
        FlyUp,  //始動
        Initialize,  //サーチ開始位置へ
        Search,  //探索
        Return,  //ロボへ戻る
        End,  //ロボの中へ戻る
    }

    [HideInInspector] public DroneDirection droneDirection = DroneDirection.Advance;
    private DroneState droneState = DroneState.Start;
    [Tooltip("前(X)、横(Z)への移動速度")] public float moveSpeed = 10.0f;
    [Tooltip("前(X)、横(Z)への移動範囲")] public Vector2 moveArea = new Vector2(70.0f, 35.0f);
    [Tooltip("Xに進む分割数")] public int XFraction = 5;
    [Tooltip("ロボットへ戻るときの移動速度")] public float returnSpeed = 20.0f;
    [Tooltip("見つけた時の追尾時間")] public float searchFollowTime = 10.0f;
    [HideInInspector] public Transform followObj = null;
    [Tooltip("ドローン出現時、上昇する速度"), SerializeField] private float flyUpSpeed = 1.0f;
    [SerializeField,Tooltip("上昇する距離")] private float upDistance = 10.0f;

    private Vector3 primaryPos;  //初期位置(x,z)
    private Vector2 onceTargetPos;  //到達したらターンする
    private float goalPos;
    private Vector3 velocity;
    private int sagittalDir;
    private int LRdir = 1;
    public Collider m_collider;
    private bool isGo; //前に進んでいるか（X）
    private float followCount;
    private bool isFirst;  //横移動の時、1回だけ半分移動する
    [SerializeField,Tooltip("自分のSnipeBulletHitAction")] private SnipeBulletHitAction snipeBullet;
    private int startEndVel = 1;
    private float upPrimary;

    // Use this for initialization
    void Start()
    {
        followCount = searchFollowTime;
        primaryPos = transform.position;
        onceTargetPos = new Vector2(BigEnemyScripts.droneSearchStartPos.position.x,
            BigEnemyScripts.droneSearchStartPos.position.z);
        Vector2 dir = (onceTargetPos - new Vector2(primaryPos.x,primaryPos.z)).normalized;
        velocity = new Vector3(dir.x, 0, dir.y);
        if (droneDirection == DroneDirection.Advance) sagittalDir = 1;
        else sagittalDir = -1;
        goalPos = transform.position.x + sagittalDir * moveArea.x;
        BigEnemyScripts.shootingPhaseMove.makebyRobot.Add(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(onceTargetPos.x, 0, onceTargetPos.y), 1);
    }

    // Update is called once per frame
    void Update()
    {
        //print(droneState);
        switch (droneState)
        {
            case DroneState.Start:
                transform.Translate(velocity * moveSpeed * Time.deltaTime);
                if ((onceTargetPos - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude <= 0.1f)
                {
                    droneState++;
                    velocity = Vector3.up;
                    upPrimary = transform.position.y + upDistance;
                }
                break;
            case DroneState.FlyUp:
                if (upPrimary < transform.position.y)
                {
                    droneState++;
                    isGo = true;
                    onceTargetPos = new Vector2(transform.position.x + sagittalDir * (moveArea.x / XFraction),
                        transform.position.z + LRdir * (moveArea.y / 2));
                    m_collider.enabled = true;
                    Vector2 dir = (onceTargetPos - new Vector2(transform.position.x, transform.position.z));
                    velocity = new Vector3(dir.x, 0, dir.y).normalized;
                }
                else
                {
                    transform.Translate(velocity * moveSpeed * Time.deltaTime);
                }
                break;
            case DroneState.Initialize:
                if ((onceTargetPos - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude <= 0.1f)
                {
                    droneState++;
                    isGo = true;
                    onceTargetPos = new Vector2(transform.position.x + sagittalDir * (moveArea.x / XFraction),
                        transform.position.z);
                    velocity = new Vector3(sagittalDir, 0, 1).normalized;
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
                if (Mathf.Abs(Mathf.Abs(goalPos) - Mathf.Abs(transform.position.x)) <= 1f)
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
                if ((new Vector2(BigEnemyScripts.droneSearchStartPos.position.x, BigEnemyScripts.droneSearchStartPos.position.z) -
                    new Vector2(transform.position.x, transform.position.z)).sqrMagnitude <= 0.1f)
                {
                    transform.parent = BigEnemyScripts.mTransform;
                    velocity = Vector3.down;
                    droneState++;
                    startEndVel = -1;
                    transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Vector3 dir = BigEnemyScripts.droneSearchStartPos.position - transform.position;
                    transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                }
                break;
            case DroneState.End:
                transform.Translate(velocity * moveSpeed * Time.deltaTime);
                if (Mathf.Abs(BigEnemyScripts.droneSearchStartPos.position.y
                       - transform.position.y) <= 1f)
                {
                    velocity = Vector3.left;
                    if ((new Vector2(transform.localPosition.x, transform.localPosition.z)).sqrMagnitude <= 3f)
                    {
                        Destroy(gameObject, 0.5f);
                    }
                }
                break;
        }
    }

    public bool IsStart
    {
        get
        {
            return (droneState == DroneState.Initialize);
        }
    }

    void OnDestroy()
    {
        BigEnemyScripts.droneCreate.RemoveDrone(gameObject);
        BigEnemyScripts.shootingPhaseMove.makebyRobot.Remove(gameObject);
    }
}
