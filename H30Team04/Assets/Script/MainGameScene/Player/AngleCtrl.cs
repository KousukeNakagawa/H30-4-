using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラの回転を司る
/// </summary>
public class AngleCtrl : MonoBehaviour
{
    GameObject player;
    PlayerBase playerScript;
    Transform car; //PlayerのTransform
    List<GameObject> snipers = new List<GameObject>(); //sniper rifle cameraをまとめる
    [SerializeField] GameObject cameraAndRifle;
    [SerializeField] GameObject sniper;
    [SerializeField] GameObject playerCamera;

    [SerializeField] GameObject captureRange;
    [SerializeField] GameObject bigEnemy;

    //カメラ回転速度
    [SerializeField, Range(1, 500)] int _rotateSpeed = 100;
    //isAim=true時のカメラ回転速度
    [SerializeField, Range(1, 100)] int aimRotateSpeed = 20;
    //前進時、前を向く速度
    [SerializeField, Range(0.1f, 10)] float lookFrontSpeed = 2;

    //上下角度範囲
    [SerializeField, Range(0, 80)] float maxAngle = 80;
    [SerializeField, Range(0, -80)] float minAngle = -25;
    //プレイヤーを透過する角度
    [SerializeField, Range(0, 90)] float hideAngle = 35;

    //カメラ反転のON/OFF
    [SerializeField] bool inverted = false;
    //前進時、前方を向くか
    [SerializeField] bool lookFront = false;
    //ロックオン妨害のON/OFF
    [SerializeField] bool isObstcle = false;

    int targetNum = 0;
    bool isLockon;
    bool isLockonEnd;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerBase>();
        car = player.transform;

        snipers.Add(cameraAndRifle);
        snipers.Add(sniper);
    }

    void Update()
    {
        if (!playerScript.GetIsEndSE()) return;

        SetChanger();

        CameraAngleControl(); //カメラアングル

        LockonTargets();
    }

    /// <summary>
    /// ＊カメラアングルのコントロール
    /// </summary>
    void CameraAngleControl()
    {
        CameraReturn();
        if (Input.GetButton("BackAngle")) return; //後ろを向いている間は後ろに固定

        RotateCameraAngle();

        //手動旋回時は自動旋回しない
        if (Input.GetAxis("CameraHorizontal") != 0 ||
            Input.GetAxis("CameraVertical") != 0) return;

        if (lookFront && !Input.GetButton("Shooting")) AutoCameraAngle();
    }

    /// <summary>
    /// ＊正面・背後の切替（右スティック押し込み）
    /// </summary>
    void CameraReturn()
    {
        foreach (var item in snipers)
        {
            //すぐに後ろを向く（180度回転）
            if (Input.GetButton("BackAngle"))
                item.transform.localEulerAngles =
                    new Vector3(item.transform.localEulerAngles.x, item.transform.localEulerAngles.y + 180, item.transform.localEulerAngles.z);

            //すぐに正面を向く
            else if (Input.GetButtonUp("BackAngle") || Input.GetButtonDown("CameraReset"))
                item.transform.localEulerAngles =
                    new Vector3(item.transform.localEulerAngles.x, item.transform.localEulerAngles.y, item.transform.localEulerAngles.z);
        }
    }

    /// <summary>
    /// ＊手動カメラアングル（右スティック）
    /// </summary>
    void RotateCameraAngle()
    {
        //カメラ反転の対応
        float changer = (inverted) ? 1 : -1;
        //AIM時の対応
        float rotateSpeed = (Input.GetButton("Shooting")) ? aimRotateSpeed : _rotateSpeed;

        //右パット入力の取得
        Vector3 angle = new Vector3(Input.GetAxis("CameraHorizontal"), Input.GetAxis("CameraVertical"));

        //sniperの回転（左右回転のみ）
        sniper.transform.eulerAngles += new Vector3(0, angle.x) * rotateSpeed * Time.deltaTime;

        //カメラとライフルの回転
        cameraAndRifle.transform.eulerAngles += new Vector3(angle.y * changer, angle.x) * rotateSpeed * Time.deltaTime;

        //-180＜上下の動き＜180に変更
        float angleX = (180 <= cameraAndRifle.transform.eulerAngles.x) ?
            cameraAndRifle.transform.eulerAngles.x - 360 : cameraAndRifle.transform.eulerAngles.x;

        //上下の制限
        cameraAndRifle.transform.eulerAngles =
            new Vector3(Mathf.Clamp(angleX, -maxAngle, -minAngle), cameraAndRifle.transform.eulerAngles.y, cameraAndRifle.transform.eulerAngles.z);

        //プレイヤーの透過
        playerCamera.GetComponent<MainCamera>().PlayerHide((angleX <= -hideAngle));
    }

    /// <summary>
    /// 進行方向を向く
    /// </summary>
    void AutoCameraAngle()
    {
        if (Input.GetAxis("Axel") > 0) //前進時
            foreach (var item in snipers)
                item.transform.rotation =
                    Quaternion.RotateTowards(item.transform.rotation, car.rotation, lookFrontSpeed);
    }

    /// <summary>
    /// ＊視点や武器の切替
    /// </summary>
    void SetChanger()
    {
        //ルックアットのON / OFF（RBボタン）
        if (Input.GetButtonDown("LockAt"))
        {
            isLockon = true;
            isLockonEnd = false;
        }
    }

    /// <summary>
    /// ＊「ドローン」「ミサイル」をロックオンする
    /// </summary>
    void LockonTargets()
    {
        //ドローンとミサイルに対する距離をソートしたリスト
        var targets = captureRange.GetComponent<AimRange>().GetTarget();
        var targetCount = targets.Count;

        //ビッグエネミーの取得
        if (!targets.ContainsValue(bigEnemy))
            targets.Add(Vector3.Distance(bigEnemy.transform.position, transform.position), bigEnemy);

        if (Input.GetButtonDown("LockAt") && isLockon) targetNum++;
        if (targetNum >= targetCount)
        {
            isLockonEnd = true;
            targetNum = -1;
        }
        if (isLockonEnd) isLockon = false;

        //Debug.Log("数：" + targetCount + "i：" + targetNum);

        if (!isLockon) return;
        if (targets.Count == 0) return;

        for (int i = 0; i < targetCount; i++)
        {
            if (targets.Values[i] == null)
            {
                targets.Clear();
                targetNum = 0;
                return;
            }

            //ターゲットとの間の障害物の有無
            bool isLookOK =
                    (Physics.Linecast(transform.position + Vector3.up, targets.Values[i].transform.position, LayerMask.GetMask("Building"))) ?
                    false : true;

            if (!isObstcle) if (!isLookOK) return;

            if (targetNum == i)
            {
                Vector3 targetPos = targets.Values[i].transform.position;
                Vector3 distance = targetPos - cameraAndRifle.transform.position;
                distance.y /= 1.5f;

                cameraAndRifle.transform.rotation =
                    Quaternion.RotateTowards(cameraAndRifle.transform.rotation, Quaternion.LookRotation(distance), lookFrontSpeed);

                //distance.y = 0;
                //sniper.transform.rotation =
                //    Quaternion.RotateTowards(sniper.transform.rotation, Quaternion.LookRotation(distance), lockonRotateSpeed);
            }

            if (targetNum == i)
            {
                Vector3 targetPos = targets.Values[i].transform.position;
                Vector3 distance = targetPos - sniper.transform.position;
                distance.y = 0;

                sniper.transform.rotation =
                    Quaternion.RotateTowards(sniper.transform.rotation, Quaternion.LookRotation(distance), lookFrontSpeed);
            }
        }
    }
}
