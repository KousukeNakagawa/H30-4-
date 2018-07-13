using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMove3 : MonoBehaviour
{
    public enum DroneDirection
    {
        Advance,  //前
        Recession  //後ろ
    }

    private enum DroneState
    {
        Start,  //出現
        Patrol,  //探索
        Search,  //プレイヤー追尾中
        Return,  //ロボへ戻る
        End,  //ロボの中へ戻る
    }

    [HideInInspector] public DroneDirection droneDirection = DroneDirection.Advance;
    [SerializeField] private DroneState droneState = DroneState.Start;
    [Tooltip("前(X)、横(Z)への移動速度")] public float moveSpeed = 10.0f;
    [Tooltip("前(X)、横(Z)への移動範囲")] public Vector2 moveArea = new Vector2(70.0f, 35.0f);
    [Tooltip("Xに進む分割数")] public int XFraction = 5;
    [Tooltip("ロボットへ戻るときの移動速度")] public float returnSpeed = 20.0f;
    [Tooltip("見つけた時の追尾時間")] public float searchFollowTime = 10.0f;
    [Tooltip("見つけた時の追尾速度")] public float searchFollowSpeed = 3.5f;
    [HideInInspector] public Transform followObj = null;
    [Tooltip("ドローン出現時、上昇する速度"), SerializeField] private float flyUpSpeed = 1.0f;
    [SerializeField, Tooltip("上昇する距離")] private float upDistance = 10.0f;

    private Vector3 primaryPos;  //初期位置(x,z)
    //private Vector2 onceTargetPos;  //到達したらターンする
    private Vector3 velocity;
    private int sagittalDir;
    //private int LRdir = 1;
    public Collider searchCollider;
    private float followCount;
    [SerializeField, Tooltip("自分のSnipeBulletHitAction")] private SnipeBulletHitAction snipeBullet;
    private SphereCollider bodySpherer;
    private List<Vector3> targetPoses = new List<Vector3>();
    private int currentNo = -1;
    private Vector3 targetPos;
    private bool isEnd;

    [SerializeField]
    Light m_light;
    [SerializeField]
    float _interval;

    // Use this for initialization
    void Start()
    {
        followCount = searchFollowTime;
        primaryPos = transform.position;
        BigEnemyScripts.shootingPhaseMove.makebyRobot.Add(gameObject);
        StartCoroutine("Blink");
        if (droneDirection == DroneDirection.Advance) sagittalDir = 1;
        else sagittalDir = -1;
        bodySpherer = GetComponent<SphereCollider>();
        TargetPosSet();
        TargetNext();
    }

    private void TargetPosSet()
    {
        int LRdir = 1;
        bool isGo = true;
        targetPoses.Clear();
        //上昇を開始する座標
        targetPoses.Add(BigEnemyScripts.droneSearchStartPos.position);
        //上昇が終了する座標
        targetPoses.Add(targetPoses[targetPoses.Count - 1] + new Vector3(0, upDistance, 0));
        //探索を開始する座標
        targetPoses.Add(targetPoses[targetPoses.Count - 1] + new Vector3(sagittalDir * (moveArea.x / XFraction),
            0, LRdir * (moveArea.y / 2)));
        //探索座標を追加
        for (int i = 0; i < XFraction * 2 - 1; i++)
        {
            isGo = !isGo;
            Vector3 pos = targetPoses[targetPoses.Count - 1];
            if (!isGo)  //次に進む方向は左右
            {
                LRdir = -LRdir;
                pos += new Vector3(0, 0, moveArea.y * LRdir);
            }
            else  //次進む方向は前
            {
                pos += new Vector3(sagittalDir * (moveArea.x / XFraction), 0, 0);
            }
            targetPoses.Add(pos);
        }
    }

    private void TargetNext()
    {
        currentNo++;
        if (currentNo == 3)
        {
            droneState = DroneState.Patrol;
            searchCollider.enabled = true;
            bodySpherer.enabled = true;
        }
        else if (currentNo == (3 + XFraction * 2 - 1))
        {
            droneState = DroneState.Return;
            searchCollider.enabled = false;
        }
        currentNo = Mathf.Clamp(currentNo, 0, (2 + XFraction * 2 - 1));
        targetPos = targetPoses[currentNo];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var p in targetPoses)
        {
            Gizmos.DrawWireSphere(p, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (droneState)
        {
            case DroneState.Start:
            case DroneState.Patrol:
                PatrolAction();
                if ((targetPos - transform.position).sqrMagnitude <= 1.0f)
                    TargetNext();
                break;
            case DroneState.Search:
                SearchAction();
                break;
            case DroneState.Return:
                ReturnAction();
                if ((BigEnemyScripts.droneSearchStartPos.position - transform.position).sqrMagnitude <= 1.0f)
                    droneState = DroneState.End;
                break;
            case DroneState.End:
                EndAction();
                break;
        }
    }

    private void SearchAction()
    {
        followCount -= Time.deltaTime;
        if (followCount <= 0)
        {
            droneState = DroneState.Return;
            return;
        }
        velocity = (followObj.position - transform.position).normalized;
        velocity.y = 0;
        transform.Translate(velocity * searchFollowSpeed * Time.deltaTime);
    }

    private void PatrolAction()
    {
        velocity = (targetPos - transform.position).normalized;
        transform.Translate(velocity * moveSpeed * Time.deltaTime, Space.World);
        if (followObj != null) droneState = DroneState.Search;
    }

    private void ReturnAction()
    {
        velocity = (BigEnemyScripts.droneSearchStartPos.position - transform.position).normalized;
        transform.Translate(velocity * returnSpeed * Time.deltaTime);
    }

    private void EndAction()
    {
        transform.parent = BigEnemyScripts.mTransform;
        if ((BigEnemyScripts.droneSearchStartPos.position - transform.position).sqrMagnitude <= 1.0f) isEnd = true;
        velocity = (isEnd) ? (BigEnemyScripts.droneInstantiatePos.position - transform.position).normalized :
            (BigEnemyScripts.droneSearchStartPos.position - transform.position).normalized;
        transform.Translate(velocity * moveSpeed * Time.deltaTime);
        if ((BigEnemyScripts.droneInstantiatePos.position - transform.position).sqrMagnitude <= 1.0f)
        {
            Destroy(gameObject);
        }
    }

    public bool IsStart
    {
        get
        {
            return (droneState == DroneState.Patrol);
        }
    }

    void OnDestroy()
    {
        BigEnemyScripts.droneCreate.RemoveDrone(gameObject);
        BigEnemyScripts.shootingPhaseMove.makebyRobot.Remove(gameObject);
    }

    IEnumerator Blink()
    {
        while (true)
        {
            m_light.enabled = !m_light.enabled;
            yield return new WaitForSeconds(_interval);
        }
    }
}
