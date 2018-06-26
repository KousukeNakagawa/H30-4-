//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class BigEnemyMove : MonoBehaviour
{
    [Tooltip("前進する速度")] public float xSpeed = 8.0f;
    [Tooltip("回転する速度")] public float turnSpeed = 40.0f;

    private Vector3 turnDir;  //回転している間の現在の角度
    private Vector3 turnEndDir;  //回転し終わった後の角度
    [HideInInspector] public bool isTurn { get; private set; }  //回転しているか
    [HideInInspector] public bool isDefense = false;

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (BigEnemyScripts.searchObject.isSearch)
        {  //探索範囲に標的が入った後、1回だけ行う
            Vector3 endDir = TurnAngleSet(BigEnemyScripts.searchObject.targetPos);
            SetTurn(endDir);
            BigEnemyScripts.searchObject.isSearch = false;
        }

        if (isTurn)
        {
            if (Mathf.Abs(turnEndDir.y - turnDir.y) <= turnSpeed * Time.deltaTime * 2.5f)
            {  //回転を終了する
                isTurn = false;
                turnDir.y = turnEndDir.y;
                BigEnemyScripts.mTransform.rotation = Quaternion.Euler(turnDir);
                BigEnemyScripts.droneCreate.DroneSet();
                if (!isDefense) BigEnemyScripts.bigEnemyAnimatorManager.isDash = true;
                if (BigEnemyScripts.missileLaunch.isMissile) BigEnemyScripts.missileLaunch.LaunchSet();
            }
            else
            {
                float speed = BigEnemyScripts.bigEnemyAnimatorManager.moveSpeed;
                turnDir.y += BigEnemyScripts.searchObject.turnVel * turnSpeed * speed * Time.deltaTime;
                turnDir = turnDir.GetUnityVector3();
                BigEnemyScripts.mTransform.rotation = Quaternion.Euler(turnDir);
            }
        }
        else
        {
            if (!BigEnemyScripts.missileLaunch.isMissile && BigEnemyScripts.droneCreate.isEnd)
            {
                BigEnemyScripts.bigEnemyAnimatorManager.WalkStart();
                float speed = BigEnemyScripts.bigEnemyAnimatorManager.moveSpeed;
                BigEnemyScripts.mTransform.Translate(speed * xSpeed * Time.deltaTime, 0, 0, Space.Self);
            }
        }
    }
    
    public Vector3 TurnAngleSet(Vector3 targetPos)
    {  //回転が終了して時の回転角度を取得する
        float dir = GetDirction(transform.position, targetPos);
        Vector3 endDir = BigEnemyScripts.mTransform.eulerAngles;
        endDir.y = Mathf.Rad2Deg * dir;
        return endDir;
    }

    private float GetDirction(Vector3 self, Vector3 target)
    {  //selfからtargetまでの角度を取得する
        Vector2 dirVec2 = (target.ToTopView() - self.ToTopView()).normalized;
        float dir = Mathf.Atan2(-dirVec2.y, dirVec2.x);
        return dir;
    }

    public void SetTurn(Vector3 endDir)
    {  //回転を開始する
        turnEndDir = endDir.GetUnityVector3();
        turnDir = BigEnemyScripts.mTransform.localEulerAngles.GetUnityVector3();
        isTurn = true;
    }

    public void SetGoDefenseLine()
    {  //防衛ラインに向かう
        BigEnemyScripts.searchObject.SetTurnVelGoDefenseLine();
        isTurn = true;
        turnDir = BigEnemyScripts.mTransform.localEulerAngles.GetUnityVector3();
        turnEndDir = Vector3.zero;
        isDefense = true;
    }
}
