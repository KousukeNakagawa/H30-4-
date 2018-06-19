using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject player;
    [SerializeField, Range(0, 10), Tooltip("プレイヤーとの距離")] float dir = 2;
    [SerializeField, Range(0, 10), Tooltip("カメラの高さ")] float height = 1.5f;
    [SerializeField, Range(1, 5), Tooltip("射影機へ視点移動時の速度")] float speed = 2;

    Vector3 basePos;
    Vector3 startAngles;

    //現在と過去のフラグ（射影機目線用）
    bool current, old;

    /// <summary> 射影機へ移動中か </summary>
    public static bool IsMove { get; private set; }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startAngles = transform.eulerAngles;
        IsMove = false;
    }

    //void Update()
    //{
    //    if (Time.timeScale == 0) return;
    //    if (!SEManager.IsEndSE) return;


    //    ShutterChance();
    //}

    void LateUpdate()
    {
        //if (Time.timeScale == 0) return;
        if (!SEManager.IsEndSE) return;

        basePos = player.transform.position - player.transform.forward * dir + Vector3.up * height;

        ShutterChance();
    }

    /// <summary> 選択中の射影機目線になる </summary>
    void ShutterChance()
    {
        current = Xray_SSS.IsShutterChance;

        UnlockManager.Unlock(UnlockState.xray);

        if (Xray_SSS.IsShutterChance)
        {
            //選択中の射影機へ移動
            var distance = (Xray_SSS.ShutterPos - transform.position);
            transform.position += distance.normalized * speed;

            //ある程度近くなったら強制到達
            if (Mathf.Abs(distance.sqrMagnitude) <= 1)
                transform.position = (Xray_SSS.ShutterPos);

            //移動しつつ射影機の角度になる
            transform.LookAt(Xray_SSS.ShutterAngle);
        }
        //通常時
        else
        {
            if (IsMove)
            {
                //プレイヤーの後ろ (basePos) へ戻る
                var distance = (basePos - transform.position);
                transform.position += distance.normalized * 1;

                //ある程度近くなったら強制到達
                if (Mathf.Abs(distance.sqrMagnitude) <= 1)
                    transform.position = basePos;

                //移動しつつ射影機の角度になる
            }
            //戻った瞬間
            //if (current != old) transform.LookAt(player.transform.position + Vector3.up);
        }

        //プレイヤーの後ろにいない間 true
        IsMove = (transform.position != basePos);

        old = current;
    }

    /// <summary> 壁抜け・床抜け防止処理 </summary>
    void AutoCameraControl()
    {
        //if (player == null) return;

        //RaycastHit hit;

        ////プレイヤーの位置からカメラにレイを飛ばし、ビルと床に衝突したら
        //if (Physics.Linecast(root.position + Vector3.up, transform.position, out hit, LayerMask.GetMask("Building")))
        //    //レイの当たった場所がカメラの位置へ
        //    transform.position =
        //        Vector3.Lerp(transform.position, hit.point, recoverySpeed * Time.deltaTime);

        ////当たっていなければ本来の位置へ戻る
        //else transform.localPosition =
        //       Vector3.Lerp(transform.localPosition, startPos, recoverySpeed * Time.deltaTime);
    }
}