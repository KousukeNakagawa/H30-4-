using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyMove : MonoBehaviour
{
    [Tooltip("前進する速度")] public float xSpeed = 8.0f;
    [Tooltip("回転する速度")] public float turnSpeed = 40.0f;

    private Vector3 turnDir;  //回転している間の現在の角度
    private Vector3 turnEndDir;  //回転し終わった後の角度
    [HideInInspector] public bool isTurn { get; private set; }  //回転しているか

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (BigEnemyScripts.searchObject.isSearch)
        {  //探索範囲に標的が入った後、1回だけ行う
            Vector3 endDir = TurnAngleSet(BigEnemyScripts.searchObject.targetPos);
            SetTurn(endDir);
            BigEnemyScripts.searchObject.isSearch = false;
            BigEnemyScripts.bigEnemyEffectManager.ChangeEffect(true);
        }

        if (isTurn)
        {
            //print(Mathf.Abs(Mathf.DeltaAngle(turnDir.y, turnEndDir.y)));
            //if (Mathf.Abs(turnEndDir.y - turnDir.y) <= turnSpeed * Time.deltaTime)
            if (Mathf.Abs(Mathf.DeltaAngle(turnDir.y, turnEndDir.y)) <= turnSpeed * Time.deltaTime)
            {  //回転を終了する
                isTurn = false;
                BigEnemyScripts.droneCreate.DroneSet();
                if (BigEnemyScripts.missileLaunch.isMissile) BigEnemyScripts.missileLaunch.LaunchSet();
                BigEnemyScripts.bigEnemyEffectManager.ChangeEffect(false);
            }
            else
            {
                turnDir.y += BigEnemyScripts.searchObject.turnVel * turnSpeed * Time.deltaTime;
                turnDir = turnDir.GetUnityVector3();
                BigEnemyScripts.mTransform.rotation = Quaternion.Euler(turnDir);
            }
        }
        else
        {
            if (!BigEnemyScripts.missileLaunch.isMissile && BigEnemyScripts.droneCreate.isEnd)
            {
                BigEnemyScripts.mTransform.Translate(xSpeed * Time.deltaTime, 0, 0, Space.Self);
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
        Vector2 dirVec2 = new Vector2(target.x - self.x, target.z - self.z).normalized;
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
        BigEnemyScripts.bigEnemyEffectManager.ChangeEffect(true);
    }
}
