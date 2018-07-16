//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class TutorialBigEnemyMove : MonoBehaviour {

    [Tooltip("前進する速度")] public float xSpeed = 8.0f;
    [Tooltip("回転する速度")] public float turnSpeed = 40.0f;

    private Vector3 turnDir;  //回転している間の現在の角度
    private Vector3 turnEndDir;  //回転し終わった後の角度
    [HideInInspector] public bool IsTurn { get;  set; }  //回転しているか
    [HideInInspector] public bool isDefense = false;
    public bool IsMove { get; set; }

    public GameObject drone;

    // Update is called once per frame
    void Update()
    {
        if (TutorialEnemyScripts.tmane.GetState() != TutorialState_T.BEACON && TutorialEnemyScripts.tmane.GetState() != TutorialState_T.SNIPER)
        {
            xSpeed = 0;
            return;
        }
        if (TutorialEnemyScripts.searchObject.isSearch)
        {  //探索範囲に標的が入った後、1回だけ行う
            Vector3 endDir = TurnAngleSet(TutorialEnemyScripts.searchObject.targetPos);
            SetTurn(endDir);
            TutorialEnemyScripts.searchObject.isSearch = false;
            xSpeed = 9.5f;
        }

        if (IsTurn)
        {
            if (Mathf.Abs(turnEndDir.y - turnDir.y) <= turnSpeed * Time.deltaTime * TutorialEnemyScripts.bigEnemyAnimatorManager.animatorSpeed)
            //if (Mathf.Abs(Mathf.DeltaAngle(turnDir.y, turnEndDir.y)) <= 1f)
            {  //回転を終了する
                IsTurn = false;
                drone.SetActive(true);
                drone.transform.parent = null;
            }
            else
            {
                float speed = TutorialEnemyScripts.bigEnemyAnimatorManager.moveSpeed;
                turnDir.y += TutorialEnemyScripts.searchObject.turnVel * turnSpeed * speed * Time.deltaTime;
                turnDir = turnDir.GetUnityVector3();
                TutorialEnemyScripts.mTransform.rotation = Quaternion.Euler(turnDir);
            }
        }
        else if(IsMove)
        {
            TutorialEnemyScripts.bigEnemyAnimatorManager.WalkStart();
            float speed = TutorialEnemyScripts.bigEnemyAnimatorManager.moveSpeed;
            TutorialEnemyScripts.mTransform.Translate(speed * xSpeed * Time.deltaTime, 0, 0, Space.Self);
            
        }
    }

    public Vector3 TurnAngleSet(Vector3 targetPos)
    {  //回転が終了して時の回転角度を取得する
        float dir = GetDirction(transform.position, targetPos);
        Vector3 endDir = TutorialEnemyScripts.mTransform.eulerAngles;
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
        turnDir = TutorialEnemyScripts.mTransform.localEulerAngles.GetUnityVector3();
        //print(turnEndDir.y + ":" + turnDir.y);
        IsTurn = true;
    }

    public void SetGoDefenseLine()
    {  //防衛ラインに向かう
        TutorialEnemyScripts.searchObject.SetTurnVelGoDefenseLine();
        IsTurn = true;
        turnDir = TutorialEnemyScripts.mTransform.localEulerAngles.GetUnityVector3();
        turnEndDir = new Vector3(0, 90, 0);
        isDefense = true;

        this.enabled = false;
    }
}
