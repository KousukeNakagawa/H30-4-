using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMainCamera : MonoBehaviour {

    GameObject player;
    [SerializeField, Range(0, 10), Tooltip("プレイヤーとの距離")] float dir = 2;
    [SerializeField, Range(0, 10), Tooltip("カメラの高さ")] float height = 1.5f;
    [SerializeField, Range(0.01f, 0.5f), Tooltip("射影機へ視点移動時の速度")] float leap = 0.1f;
    float backupLeap;
    float speed = 0;

    Vector3 basePos;
    Vector3 startAngles;
    Vector3 backUpAngle;

    //現在と過去のフラグ（射影機目線用）
    bool current, old;
    bool cur, ol;

    /// <summary> 射影機へ移動中か </summary>
    public static bool IsMove { get; private set; }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startAngles = transform.eulerAngles;
        IsMove = false;
        backupLeap = leap;
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0) return;

        basePos = player.transform.position - player.transform.forward * dir + Vector3.up * height;

        ShutterChance();
    }

    /// <summary> 選択中の射影機目線になる </summary>
    void ShutterChance()
    {
        current = IsMove;
        cur = TutorialXray_SSS.IsShutterChance;

        //UnlockManager.Unlock(UnlockState.xray); //後で消す

        if (TutorialXray_SSS.IsShutterChance)
        {
            leap += backupLeap;
            //選択中の射影機へ移動
            var distance = (TutorialXray_SSS.ShutterPos - transform.position);
            transform.position = Vector3.Lerp(transform.position, TutorialXray_SSS.ShutterPos, (speed + leap) * Time.deltaTime);

            //ある程度近くなったら強制到達
            if (Mathf.Abs(distance.sqrMagnitude) <= 0.01f)
            {
                transform.position = (TutorialXray_SSS.ShutterPos);
                leap = backupLeap;
            }

            //移動しつつ射影機の角度になる
            transform.LookAt(TutorialXray_SSS.ShutterAngle);
        }
        //通常時
        else
        {
            if (IsMove)
            {
                leap += backupLeap;
                //プレイヤーの後ろ (basePos) へ戻る
                var distance = (basePos - transform.position);
                transform.position = Vector3.Lerp(transform.position, basePos, (speed + leap) * Time.deltaTime);

                //ある程度近くなったら強制到達
                if (Mathf.Abs(distance.sqrMagnitude) <= 0.01f)
                {
                    transform.position = basePos;
                    leap = backupLeap;
                }

                //移動しつつ射影機の角度になる
                transform.LookAt(player.transform.position + player.transform.forward + Vector3.up);
            }
            //プレイヤーに追従させる処理
            else transform.position = basePos;

            //元の位置に戻った瞬間
            if (!current && old)
                transform.LookAt(player.transform.position + player.transform.forward + Vector3.up);
        }

        if (cur != ol) leap = backupLeap;

        //プレイヤーの後ろにいない間 true
        IsMove = (transform.position != basePos);
        ol = cur;
        old = current;
    }
}
