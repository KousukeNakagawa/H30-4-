using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    /*** 必要ゲームオブジェクト ***/
    /// <summary> プレイヤー </summary>
    GameObject player;

    /*** カメラ情報 ***/
    /// <summary> プレイヤーとの距離 </summary>
    [SerializeField, Range(0, 10), Tooltip("プレイヤーとの距離")] float dir = 2;
    /// <summary> カメラの高さ </summary>
    [SerializeField, Range(0, 10), Tooltip("カメラの高さ")] float height = 1.5f;
    /// <summary> 視点移動速度補間 </summary>
    [SerializeField, Range(1, 5), Tooltip("射影機へ視点移動時の速度")] float leap = 3;

    /// <summary> 視点移動速度補間の初期値 </summary>
    float backupLeap;
    /// <summary> 視点移動速度 </summary>
    float speed = 0;

    /// <summary> プレイヤーの後方位置 </summary>
    Vector3 basePos;
    /// <summary> 前フレームにプレイヤーの後方位置にいなかったか </summary>
    bool IsNotOldPlayerLook;
    /// <summary> 前フレームは射影機目線だったか </summary>
    bool IsOldXrayLook;

    /*** プロパティ ***/
    /// <summary> 射影機へ移動中か </summary>
    public static bool IsNotPlayerBack { get; private set; }

    /// <summary> 射影機からプレイヤーに戻っている最中か </summary>
    public static bool IsComeBack { get; private set; }

    void Start()
    {
        // プレイヤーの取得
        player = GameObject.FindGameObjectWithTag("Player");
        IsNotPlayerBack = false;
        IsComeBack = false;
        backupLeap = leap;
    }

    void LateUpdate()
    {
        // 一時停止中は何もしない
        if (Time.timeScale == 0) return;
        //if (!SEManager.IsEndSE) return;

        // 基本位置の指定（プレイヤーの後ろ）
        basePos = player.transform.position - player.transform.forward * dir + Vector3.up * height;

        // 射影機目線処理
        ShutterChance();
    }

    /// <summary> 選択中の射影機目線になる </summary>
    void ShutterChance()
    {
        // 基本 false（基本プレイヤーの後ろにいるため）
        IsComeBack = false;

        // LTを押している間
        if (Xray_SSS.IsShutterChance)
        {
            // カメラから選択中の射影機までの距離
            var distance = (Xray_SSS.ShutterPos - transform.position);

            // 補間処理
            leap += backupLeap;
            // 選択中の射影機へ移動
            transform.position = Vector3.Lerp(transform.position, Xray_SSS.ShutterPos, (speed + leap) * Time.deltaTime);

            // ある程度選択中の射影機と近くなったら
            if (Mathf.Abs(distance.sqrMagnitude) <= 0.01f)
                // 強制到達
                transform.position = (Xray_SSS.ShutterPos);

            // 選択中の射影機の角度になる
            transform.LookAt(Xray_SSS.ShutterAngle);
        }
        // 通常時
        else
        {
            // プレイヤーの後ろにいない間
            if (IsNotPlayerBack)
            {
                // カメラの位置からプレイヤーの後ろまでの距離
                var distance = (basePos - transform.position);

                // 戻るフラグを立てる
                IsComeBack = true;
                // 補間処理
                leap += backupLeap;
                // プレイヤーの後ろへ移動
                transform.position = Vector3.Lerp(transform.position, basePos, (speed + leap) * Time.deltaTime);

                // ある程度近くなったら
                if (Mathf.Abs(distance.sqrMagnitude) <= 0.01f)
                    // 強制到達
                    transform.position = basePos;

                // プレイヤーの進行方向の角度になる
                transform.LookAt(player.transform.position + player.transform.forward + Vector3.up);
            }
            // プレイヤーに追従させる処理 下を向いていない時
            else if (!Soldier.IsDownLook)
                transform.position = Vector3.Lerp(transform.position, basePos, (speed + leap) * Time.deltaTime);

            // プレイヤーの後ろに戻った瞬間
            if (transform.position != basePos && !IsNotOldPlayerLook)
                // プレイヤーの進行方向を見る
                transform.LookAt(player.transform.position + player.transform.forward + Vector3.up);
        }

        // 現在と前フレームで LTの入力が異なった瞬間
        if (Xray_SSS.IsShutterChance != IsOldXrayLook)
            // 補間を初期値へ
            leap = backupLeap;

        /*** 前フレームの更新 ***/
        // プレイヤーの後ろにいない間 かつ下を見ていないか
        IsNotPlayerBack = (transform.position != basePos && !Soldier.IsDownLook);
        // LTを押しているか
        IsOldXrayLook = Xray_SSS.IsShutterChance;
        // プレイヤーの後方にいないか
        IsNotOldPlayerLook = transform.position != basePos;
    }

    /// <summary> カメラが定位置にあり LTも押していない (カメラが戻ってきたか) </summary>
    public static bool IsComeBackCamera()
    {
        return (!IsNotPlayerBack && !Xray_SSS.IsShutterChance);
    }

    ///// <summary> 壁抜け・床抜け防止処理 </summary>
    //void AutoCameraControl()
    //{
    //    //if (player == null) return;

    //    //RaycastHit hit;

    //    ////プレイヤーの位置からカメラにレイを飛ばし、ビルと床に衝突したら
    //    //if (Physics.Linecast(root.position + Vector3.up, transform.position, out hit, LayerMask.GetMask("Building")))
    //    //    //レイの当たった場所がカメラの位置へ
    //    //    transform.position =
    //    //        Vector3.Lerp(transform.position, hit.point, recoverySpeed * Time.deltaTime);

    //    ////当たっていなければ本来の位置へ戻る
    //    //else transform.localPosition =
    //    //       Vector3.Lerp(transform.localPosition, startPos, recoverySpeed * Time.deltaTime);
    //}
}