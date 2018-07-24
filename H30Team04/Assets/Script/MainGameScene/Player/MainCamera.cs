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
    [SerializeField, Range(1, 15), Tooltip("射影機へ視点移動時の速度")] float leap = 3;

    /// <summary> 視点移動速度補間の初期値 </summary>
    float backupLeap;
    /// <summary> 視点移動速度 </summary>
    float speed_ = 0;

    /// <summary> プレイヤーの後方位置 </summary>
    Vector3 playerBack;
    /// <summary> カメラの角度バックアップ </summary>
    Vector3 backupAngle_;
    /// <summary> 前フレームにプレイヤーの後方位置にいなかったか </summary>
    bool IsNotOldPlayerLook;
    /// <summary> 前フレームは射影機目線だったか </summary>
    bool IsOldXrayLook;

    /*** プロパティ ***/
    /// <summary> 射影機へ移動中か </summary>
    public static bool IsNotPlayerBack { get; private set; }

    /// <summary> 射影機からプレイヤーに戻っている最中か </summary>
    public static bool IsComeBack { get; private set; }

    /// <summary> 指定された位置にいるか </summary>
    bool isReturnPosition;
    /// <summary> いるべき位置 </summary>
    Vector3 basePosition_;

    void Start()
    {
        // プレイヤーの取得
        player = GameObject.FindGameObjectWithTag("Player");
        IsNotPlayerBack = false;
        backupLeap = leap;
    }

    void LateUpdate()
    {
        // 一時停止中は何もしない
        if (Time.timeScale == 0) return;
        //if (!SEManager.IsEndSE) return;

        // プレイヤーの後ろの位置を取得
        playerBack = player.transform.position - player.transform.forward * dir + Vector3.up * height;
        // 基本の位置を決める
        basePosition_ = (Soldier.IsDownLook) ? Soldier.FpsPos_ : playerBack;

        // 射影機目線処理
        CameraMove();
    }

    /// <summary> カメラの移動処理 </summary>
    void CameraMove()
    {
        // 射影機目線になれるか（LTを押していない時 カメラが基本位置に戻っている最中なら）
        IsComeBack = (IsNotPlayerBack && !Xray_SSS.IsShutterChance);

        // LTを押している間
        if (Xray_SSS.IsShutterChance)
        {
            ShutterMode();
        }
        // 通常時
        else
        {
            // プレイヤーの後ろにいない間
            if (IsNotPlayerBack)
            {
                ReturnBasePosition();
            }
            // プレイヤーに追従させる処理 下を向いていない時
            else if (!Soldier.IsDownLook)
                transform.position = basePosition_;

            // プレイヤーの後ろに戻った瞬間
            if (transform.position != basePosition_ && !IsNotOldPlayerLook)
                // プレイヤーの進行方向を見る
                transform.LookAt(transform.position + player.transform.forward + Vector3.up * 0.1f);
        }

        // 現在と前フレームでLTの入力が異なった瞬間
        if (Xray_SSS.IsShutterChance != IsOldXrayLook)
        {
            // 補間値を初期値へ
            //leap = backupLeap;
        }

        // LTを押していないとき 前フレームもLTを押していなかった時
        if (!Xray_SSS.IsShutterChance && !IsOldXrayLook)
        {
            //角度のバックアップ
            backupAngle_ = transform.forward + transform.position;
        }

        /*** 前フレームの更新 ***/
        // プレイヤーの後ろにいない間 かつ下を見ていないか
        IsNotPlayerBack = transform.position != basePosition_;
        // LTを押しているか
        IsOldXrayLook = Xray_SSS.IsShutterChance;
        // プレイヤーの後方にいないか
        IsNotOldPlayerLook = transform.position != basePosition_;
    }

    /// <summary> 射影機目線処理 </summary>
    void ShutterMode()
    {
        /*** 移動処理 ***/
        // 選択中の射影機へ移動する
        transform.position = Vector3.Lerp(transform.position, Xray_SSS.ShutterPos, leap * Time.deltaTime);

        /*** 角度処理 ***/
        transform.LookAt(Xray_SSS.ShutterAngle);

        /*** 到達処理 ***/
        // 現在の位置と選択中の射影機の位置までの距離
        var distance = (Xray_SSS.ShutterPos - transform.position);
        // 選択中の射影機にある程度近づいたら
        if (Mathf.Abs(distance.sqrMagnitude) <= 0.01f)
        {
            // 強制到達
            transform.position = Xray_SSS.ShutterPos;
        }
    }

    /// <summary> 基本位置に戻る処理 </summary>
    void ReturnBasePosition()
    {
        // 戻る位置
        var returnPosition = (Soldier.IsDownLook) ? Soldier.FpsPos_ : playerBack;

        /*** 移動処理 ***/
        // 基本位置へ移動する
        transform.position = Vector3.Lerp(transform.position, returnPosition, leap);

        /*** 角度処理 ***/
        // 移動する前の角度に戻る
        transform.LookAt(transform.position + player.transform.forward + Vector3.up * 0.1f);

        /*** 到達処理 ***/
        // 現在の位置と基本位置の位置までの距離
        var distance = (returnPosition - transform.position);
        // 基本位置にある程度近づいたら
        if (Mathf.Abs(distance.sqrMagnitude) <= 0.01f)
        {
            // 強制到達
            transform.position = returnPosition;
        }
    }

    /// <summary> カメラが定位置にあり LTも押していない (カメラが戻ってきたか) </summary>
    public static bool IsComeBackCamera()
    {
        return (!IsNotPlayerBack && !Xray_SSS.IsShutterChance);
    }
}