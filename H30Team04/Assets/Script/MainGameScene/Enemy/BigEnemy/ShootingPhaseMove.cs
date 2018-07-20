//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPhaseMove : MonoBehaviour
{
    [Tooltip("射撃フェーズ時に停止しないBehaviour"), SerializeField] private MonoBehaviour[] scripts;
    [Tooltip("射撃フェーズ時に停止するBehaviour"), SerializeField] private Behaviour[] contents;
    [Header("Y座標は関係ありません")]
    [SerializeField, Tooltip("射撃フェーズでの目標座標")] private Vector3 targetPos;
    [Tooltip("射撃フェーズでの移動スピード")] public float moveSpeed = 1.0f;
    [HideInInspector] public List<GameObject> makebyRobot = new List<GameObject>();

    [HideInInspector] public bool isShooting;

    // Update is called once per frame
    void Update()
    {
        //デバッグ用
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isShooting) ShootingPhaseSet();
            else BigEnemyScripts.shootingFailure.FailureAction();
        }
#endif
        if (!isShooting || Time.timeScale == 0) return;
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.Self);

    }

    public void ShootingPhaseSet()
    {
        //全てのスクリプトを取得
        List<MonoBehaviour> scs = new List<MonoBehaviour>(GetComponentsInChildren<MonoBehaviour>());
        //例外は消す
        foreach (var script in scripts)
        {
            scs.RemoveAll((f => f.Equals(script)));
        }
        //スクリプトを止める
        foreach (var script in scs)
        {
            script.enabled = false;
        }
        //他のものを止める
        foreach (var com in contents)
        {
            com.enabled = false;
        }
        //ロボットが作ったオブジェクトを削除する
        foreach (var make in makebyRobot)
        {
            Destroy(make);
        }
        //射撃フェーズへ移行する
        isShooting = true;
        //座標を取得
        Vector3 pos = transform.position;
        //座標を移動する
        pos.x = targetPos.x - MainStageDate.TroutLengthX;
        pos.z = targetPos.z;
        transform.position = pos;
        //TargetPosの方向へ向く
        BigEnemyScripts.mTransform.rotation = Quaternion.Euler(BigEnemyScripts.bigEnemyMove.TurnAngleSet(targetPos));
        //アニメーションを射撃フェーズへ移行する
        BigEnemyScripts.bigEnemyAnimatorManager.ShootingMove();
    }
}
