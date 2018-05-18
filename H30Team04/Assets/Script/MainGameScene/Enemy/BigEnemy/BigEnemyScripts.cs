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

    public static Transform droneInstantiatePos;
    public static Transform droneSearchStartPos;

    [SerializeField] private Transform[] transforms;

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
        mTransform = transform;
        droneInstantiatePos = transform.Find("DroneInstantiate");
        droneSearchStartPos = transform.Find("DroneSearchStartPos");
    }

    void Update()
    {
        foreach (Transform child in transforms)
        {
            child.position = mTransform.position;
        }
    }
}
