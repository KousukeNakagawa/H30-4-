using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    Vector3 P_Pos;

    //プレイヤー追尾機能用
    Vector3 posVector;
    float modify;
    float distance;

    //カメラの移動用
    float startRotationX;
    float rotate;
    float V_Rotate;
    float H_Rotate;
    bool isRock;


    void Start()
    {
        P_Pos = player.transform.position;
        startRotationX = transform.localRotation.x;
        rotate = 100;
        modify = 5f;
        isRock = false;
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, P_Pos);

        if (V_Rotate != 0 || H_Rotate != 0 || isRock) HomingPlayer();
        else HomingPlayerBack();

        if (Input.GetButtonDown("Rock") && !isRock) isRock = true;
        else if (Input.GetButtonDown("Rock") && isRock) isRock = false;

        VerticalCameraWork();
        HorizontalCameraWork();
    }

    /// <summary>
    /// ＊プレイヤーを背後から追尾
    /// </summary>
    void HomingPlayerBack()
    {
        Vector3 playerPos = player.transform.position; //最新のプレイヤー位置情報の取得

        //カメラアングル
        Vector3 backVector = (P_Pos - playerPos).normalized;
        backVector.y *= 0; //地面抜けバグ回避用
        posVector = (backVector == Vector3.zero) ? posVector : backVector;

        //プレイヤーの追尾
        float speed = distance * 3; //カメラ速度は距離に比例
        Vector3 targetPos = playerPos + posVector * modify + Vector3.up; //焦点
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        transform.LookAt(playerPos);

        P_Pos = player.transform.position; //プレイヤー位置情報の更新
    }

    void HomingPlayer()
    {
        transform.position += player.transform.position - P_Pos;
        P_Pos = player.transform.position;
    }

    /// <summary>
    /// ＊カメラの上下移動
    /// </summary>
    void VerticalCameraWork()
    {
        float cameraVertical = Input.GetAxis("CameraVertical");
        V_Rotate = cameraVertical; //中継

        //if (0f <= transform.localRotation.x && transform.localRotation.x <= 0.4f) //範囲設定
        float V_rotate = -cameraVertical * Time.deltaTime * rotate;
        V_rotate = Mathf.Clamp(V_rotate, -0.4f, 0.2f);
        transform.RotateAround(P_Pos, transform.right, V_rotate);
    }

    void HorizontalCameraWork()
    {
        float cameraHorizontal = Input.GetAxis("CameraHorizontal");
        H_Rotate = cameraHorizontal; //中継
        transform.RotateAround(P_Pos, Vector3.up, cameraHorizontal * Time.deltaTime * rotate * 2);
    }
}
