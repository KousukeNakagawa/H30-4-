using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPhaseMove : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] scripts;
    [SerializeField] private Behaviour[] contents;
    [Header("Y座標は関係ありません")]
    [SerializeField, Tooltip("射撃フェーズでの目標座標")] private Vector3 targetPos;
    [Tooltip("射撃フェーズでの移動スピード")] public float moveSpeed = 1.0f;
    [HideInInspector] public List<GameObject> makebyRobot = new List<GameObject>();

    [HideInInspector] public bool isShooting;
    [SerializeField, Tooltip("走るエフェクト")] private GameObject runEffect;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShootingPhaseSet();
        }
        if (!isShooting) return;
    }

    public void ShootingPhaseSet()
    {
        //射撃フェーズへ移動
        List<MonoBehaviour> scs = new List<MonoBehaviour>(GetComponentsInChildren<MonoBehaviour>());
        foreach (var script in scripts)
        {
            scs.RemoveAll((f => f.Equals(script)));
        }
        //スクリプトを止める
        foreach (var script in scs)
        {
            script.enabled = false;
        }
        foreach (var com in contents)
        {
            com.enabled = false;
        }
        //ロボットが作ったオブジェクトを削除する
        foreach (var make in makebyRobot)
        {
            Destroy(make);
        }
        isShooting = true;
        Vector3 pos = transform.position;
        pos.z = targetPos.z;
        transform.position = pos;
        BigEnemyScripts.mTransform.rotation = Quaternion.Euler(BigEnemyScripts.bigEnemyMove.TurnAngleSet(targetPos));
        BigEnemyScripts.shootingFailure.FailureAction();
    }
}
