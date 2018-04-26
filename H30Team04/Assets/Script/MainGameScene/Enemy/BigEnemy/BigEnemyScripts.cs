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
    public static Transform mTransform;

    [SerializeField] private Transform[] transforms;

    // Use this for initialization
    void Start()
    {
        bigEnemyMove = GetComponentInChildren<BigEnemyMove>();
        searchObject = GetComponentInChildren<SearchObject>();
        destinationArrive = GetComponentInChildren<DestinationArrive>();
        missileLaunch = GetComponentInChildren<MissileLaunch>();
        mTransform = transform;
    }

    void Update()
    {
        foreach (Transform child in transforms)
        {
            child.position = mTransform.position;
        }
    }
}
