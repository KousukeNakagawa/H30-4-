using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //必須ゲームオブジェクト
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject FPSCamera;
    [SerializeField] GameObject bigEnemy;
    [SerializeField] GameObject beacon;
    [SerializeField] GameObject bullet;

    //移動・旋回の速度
    [SerializeField] [Range(0.1f, 10f)] float homingSpeed = 2;
    [SerializeField] [Range(50, 500)] float angleRotateSpeed = 100;
    [SerializeField] [Range(1, 500)] float FpsRotateSpeed = 50;
    [SerializeField] [Range(0.1f, 10f)] float autoLookSpeed = 1;
    [SerializeField] [Range(0.1f, 10f)] float lockonRotateSpeed = 10;

    //上下アングルの制限範囲
    [SerializeField] [Range(-90, 0)] float lowAngleLimit = -30;
    [SerializeField] [Range(0, 90)] float highAngleLimit = 80;

    Transform car; //プレイヤーのトランスフォーム仲介役

    bool isLockAt_Big; //ビッグエネミーLook用
    bool isBackAngle; //カメラの前後切替フラグ
    bool isSnipe;
    bool isWeaponBeacon;

    void Start()
    {
        car = player.transform;
        isLockAt_Big = false;
        isBackAngle = false;
        isSnipe = false;
        isWeaponBeacon = true;
    }

    void Update()
    {
        HomingPlayer(isSnipe); //移動

        CameraAngleControl(isSnipe); //カメラアングル

        CameraModeChange(); //カメラモード切替

        ShootingMode();
    }

    /// <summary>
    /// ＊プレイヤーの追尾
    /// </summary>
    void HomingPlayer(bool fps)
    {
        //通常時：少し遅れる //FPS時：遅れない
        transform.position = (fps) ?
            car.position : Vector3.Lerp(transform.position, car.position, homingSpeed);
    }

    /// <summary>
    /// ＊カメラアングルのコントロール
    /// </summary>
    void CameraAngleControl(bool fps)
    {
        CameraReturn();
        RotateCameraAngle(fps);

        if (fps) return; //FPS視点の時は自動旋回しない

        if (isBackAngle) return; //後ろを向いている間は後ろに固定

        //手動旋回時は自動旋回しない
        if (Input.GetAxis("CameraHorizontal") != 0 ||
            Input.GetAxis("CameraVertical") != 0) return;

        AutoCameraAngle();
    }

    /// <summary>
    /// ＊自動カメラアングル
    /// </summary>
    void AutoCameraAngle()
    {
        //ビッグエネミーとの間の障害物の有無
        bool isLookOK =
            (Physics.Linecast(transform.position + Vector3.up, bigEnemy.transform.position, LayerMask.GetMask("Default"))) ?
            false : true;

        //プレイヤー目線
        Vector3 fromPlayerAngle = bigEnemy.transform.position - transform.position;

        if (isLockAt_Big && isLookOK) //ビッグエネミーを向く
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(fromPlayerAngle), lockonRotateSpeed);

        else if (Input.GetAxis("Axel") >= 0) //ルック状態じゃなければ徐々に前方へカメラを向ける
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, car.rotation, autoLookSpeed);

        else transform.LookAt(car.position); //バック時は後ろを見る
    }

    /// <summary>
    /// ＊正面・背後の切替（右スティック押し込み）
    /// </summary>
    void CameraReturn()
    {
        //すぐに後ろを向く（180度回転）
        if (Input.GetButtonDown("BackAngle") && !isBackAngle)
            transform.localEulerAngles =
                new Vector3(transform.localEulerAngles.x, car.localEulerAngles.y + 180, transform.localEulerAngles.z);

        //すぐに正面を向く
        else if ((Input.GetButtonDown("BackAngle") && isBackAngle) || Input.GetButtonDown("CameraReset"))
            transform.localEulerAngles =
                new Vector3(transform.localEulerAngles.x, car.localEulerAngles.y, transform.localEulerAngles.z);
    }

    /// <summary>
    /// ＊手動カメラアングル（右スティック）
    /// </summary>
    void RotateCameraAngle(bool fps)
    {
        //メインカメラとFPSの時の変換処理
        float changer = -1;
        float rotateSpeed = angleRotateSpeed;
        float downLimit = lowAngleLimit;
        float upLimit = highAngleLimit;
        if (fps)
        {
            //changer = 1;
            rotateSpeed = FpsRotateSpeed;
            //downLimit = -highAngleLimit;
            //upLimit = -lowAngleLimit;
        }

        //右パッドの登録
        Vector3 angle =
            new Vector3(Input.GetAxis("CameraHorizontal"), Input.GetAxis("CameraVertical"), 0);

        //上下左右360度回転（更に下のコードで上下の動きを制限）
        transform.eulerAngles += new Vector3(angle.y * changer, angle.x) * rotateSpeed * Time.deltaTime;

        //-180＜上下の動き＜180に変更
        float angleX = 180 <= transform.eulerAngles.x ?
            transform.eulerAngles.x - 360 : transform.eulerAngles.x;

        //更に上下の動きの制限
        transform.eulerAngles =
            new Vector3(Mathf.Clamp(angleX, downLimit, upLimit), transform.eulerAngles.y, transform.eulerAngles.z);
    }

    /// <summary>
    /// ＊視点の切替
    /// </summary>
    void CameraModeChange()
    {
        //カメラの正面・背後の切替
        if (Input.GetButtonDown("BackAngle")) isBackAngle = !isBackAngle;

        //ルックアットのON/OFF（RBボタン）
        if (Input.GetButtonDown("LockAt")) isLockAt_Big = !isLockAt_Big;

        //FPS視点の切替（LB）
        if (Input.GetButtonDown("Shooting"))
        {
            //mainCamera.SetActive(!mainCamera.activeSelf);
            //FPSCamera.SetActive(!FPSCamera.activeSelf);
            isSnipe = true;
        }
        if (Input.GetButtonUp("Shooting")) isSnipe = false;
    }

    /// <summary>
    /// ＊FPS視点時のアクション
    /// </summary>
    void ShootingMode()
    {
        RaycastHit hit;
        Ray ray = new Ray(FPSCamera.transform.position, transform.forward);
        float rayLength = (isWeaponBeacon) ? 50 : 100;
        Color rayColor = (isWeaponBeacon) ? Color.blue : Color.red;

        if (Physics.Raycast(ray))
        {

        }

        if (Input.GetButtonDown("Fire"))
        {
            //発射する武器
            GameObject weapon = (isWeaponBeacon) ? Instantiate(beacon) : Instantiate(bullet);
            //初期位置
            weapon.transform.position = FPSCamera.transform.position;
            //発射
            if (isWeaponBeacon)
                weapon.GetComponent<Beacon>().Fire(transform.forward);
            else
                weapon.GetComponent<SniperBullet>().Fire(transform.forward);
        }

        //武器の切替
        if (Input.GetButtonDown("WeaponChange")) isWeaponBeacon = !isWeaponBeacon;

        Debug.DrawRay(ray.origin, ray.direction * rayLength, rayColor, 0, true);
    }
}
