using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyScripts : MonoBehaviour
{
    //巨大ロボのスクリプトにアクセスしやすいようにするためのまとめ
    //staticを使っているため巨大ロボを2体以上出したらエラーが発生する
    public static BigEnemyMove bigEnemyMove;
    public static SearchObject searchObject;
    public static DestinationArrive destinationArrive;
    public static MissileLaunch missileLaunch;
    public static DroneCreate droneCreate;
    public static ShootingPhaseMove shootingPhaseMove;
    public static Transform mTransform;
    public static BigEnemyEffectManager bigEnemyEffectManager;
    public static ShootingFailure shootingFailure;
    public static BreakEffectManager breakEffectManager;
    public static MissileGroupManager missileGroupManager;
    public static BigEnemyAnimatorManager bigEnemyAnimatorManager;
    public static BigEnemyEffect bigEnemyEffect;
    public static BigEnemyAudioManager bigEnemyAudioManager;

    //ドローンを出す座標
    public static Transform droneInstantiatePos;
    //ドローンが上昇して探索を開始する座標
    public static Transform droneSearchStartPos;

    [SerializeField] private Transform[] transforms;
    //ボディ
    [SerializeField] private Transform body_;
    //ローカルポジション固定
    private Dictionary<Transform, Vector3> localPoses = new Dictionary<Transform, Vector3>();

    void Awake()
    {
        mTransform = transform;
    }

    // Use this for initialization
    void Start()
    {
        bigEnemyMove = GetComponentInChildren<BigEnemyMove>();
        searchObject = GetComponentInChildren<SearchObject>();
        destinationArrive = GetComponentInChildren<DestinationArrive>();
        missileLaunch = GetComponentInChildren<MissileLaunch>();
        droneCreate = GetComponentInChildren<DroneCreate>();
        shootingPhaseMove = GetComponent<ShootingPhaseMove>();
        bigEnemyEffectManager = GetComponentInChildren<BigEnemyEffectManager>();
        shootingFailure = GetComponent<ShootingFailure>();
        breakEffectManager = GetComponentInChildren<BreakEffectManager>();
        missileGroupManager = GetComponentInChildren<MissileGroupManager>();
        bigEnemyAnimatorManager = GetComponentInChildren<BigEnemyAnimatorManager>();
        bigEnemyAudioManager = GetComponent<BigEnemyAudioManager>();
        bigEnemyEffect = GetComponentInChildren<BigEnemyEffect>();
        droneInstantiatePos = transform.Find("DroneInstantiate");
        droneSearchStartPos = transform.Find("DroneSearchStartPos");
        foreach (Transform child in transform)
        {
            localPoses[child] = child.localPosition;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        foreach (Transform child in transform)
        {
            if (localPoses.ContainsKey(child)) child.localPosition = localPoses[child];
        }
    }
}
