using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //必須ゲームオブジェクト
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject FPSCamera;
    [SerializeField] GameObject aimRange;
    [SerializeField] GameObject beaconBullet;
    [SerializeField] GameObject snipeBullet;
    [SerializeField] GameObject blueRippel;
    [SerializeField] GameObject redRippel;
    [SerializeField] GameObject sniper;

    [SerializeField, Range(0.1f, 1f)] float homingSpeed = 1; //追従速度
    [SerializeField, Range(50, 500)] float angleRotateSpeed = 100; //手動アングル速度
    [SerializeField, Range(1, 500)] float fpsRotateSpeed = 50; //FPS時アングル速度
    [SerializeField, Range(0.1f, 10f)] float autoLookSpeed = 1; //自動修正アングル速度
    [SerializeField, Range(0.1f, 10f)] float lockonRotateSpeed = 10; //振り向き速度

    //上下アングルの制限範囲
    [SerializeField, Range(-90, 0)] float lowAngleLimit = -30;
    [SerializeField, Range(0, 90)] float highAngleLimit = 80;

    Transform car; //プレイヤーのトランスフォーム

    bool isLockAt_Big; //ビッグエネミーLook用
    bool isBackAngle; //カメラの前後切替フラグ
    bool isSnipe; //射撃モードのオンオフ
    bool isWeaponBeacon; //武器の切替用

    int targetNum;

    void Start()
    {
        car = player.transform;
        isLockAt_Big = false;
        isBackAngle = false;
        isSnipe = false;
        isWeaponBeacon = true;

        targetNum = 0;
    }

    void Update()
    {
        if (player == null) return;

        HomingPlayer(isSnipe); //移動

        CameraAngleControl(isSnipe); //カメラアングル

        SetChanger(); //カメラモード切替

        ShootingMode();

        //LockonTarget();

        LockonTargets();
    }

    /// <summary>
    /// ＊プレイヤーの追尾移動
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

        //if (fps) return; //FPS視点の時は自動旋回しない

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
        ////ビッグエネミーの取得
        //var bigEnemy = GameObject.FindGameObjectWithTag("BigEnemy").transform;

        ////ビッグエネミーとの間の障害物の有無
        //bool isLookOK =
        //    (Physics.Linecast(transform.position + Vector3.up, bigEnemy.position, LayerMask.GetMask("Building"))) ?
        //    false : true;

        ////プレイヤー目線
        //Vector3 fromPlayerAngle = bigEnemy.position - transform.position;

        //if (isLockAt_Big && isLookOK) //ビッグエネミーを向く
        //    transform.rotation =
        //        Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(fromPlayerAngle), lockonRotateSpeed);

        ////else if (Input.GetAxis("Axel") >= 0) //ルック状態じゃなければ徐々に前方へカメラを向ける
        ////    transform.rotation =
        ////        Quaternion.RotateTowards(transform.rotation, car.rotation, autoLookSpeed);

        //else transform.LookAt(car.position); //バック時は後ろを見る
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
            rotateSpeed = fpsRotateSpeed;
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
    /// ＊視点や武器の切替
    /// </summary>
    void SetChanger()
    {
        //カメラの正面・背後の切替
        if (Input.GetButtonDown("BackAngle")) isBackAngle = true;
        else if (Input.GetButtonUp("BackAngle")) isBackAngle = false;

        //ルックアットのON/OFF（RBボタン）
        if (Input.GetButton("LockAt")) isLockAt_Big = true;
        else if (Input.GetButtonUp("LockAt")) isLockAt_Big = false;

        //FPS視点の切替（LB）
        if (Input.GetButtonDown("Shooting"))
        {
            //mainCamera.SetActive(!mainCamera.activeSelf);
            //FPSCamera.SetActive(!FPSCamera.activeSelf);
            isSnipe = true;
        }
        if (Input.GetButtonUp("Shooting")) isSnipe = false;
        //武器の切替
        if (Input.GetButtonDown("WeaponChange")) isWeaponBeacon = !isWeaponBeacon;
    }

    /// <summary>
    /// ＊射撃モード
    /// </summary>
    void ShootingMode()
    {
        RaycastHit hit; //レイとの衝突場所にエフェクトを後々追加予定

        //自分の位置の少し上から正面へ（微調整）
        Ray ray = new Ray(transform.position + Vector3.up * 1.8f, transform.forward /*- Vector3.up / 50*/);

        //武器によって長さが変わる
        float rayLength = (isWeaponBeacon) ?
            beaconBullet.GetComponent<BeaconBullet>().GetRangeDistance() : snipeBullet.GetComponent<SniperBullet>().GetRangeDistance();

        //武器によって色が変わる
        Color rayColor = (isWeaponBeacon) ? Color.blue : Color.red;

        GameObject effect = (isWeaponBeacon) ? blueRippel : redRippel;



        if (isWeaponBeacon)
        {
            blueRippel.SetActive(true);
            redRippel.SetActive(false);
        }
        else
        {
            blueRippel.SetActive(false);
            redRippel.SetActive(true);
        }

        if (Input.GetButtonDown("Fire")) Fire(ray); //射撃

        //レイの表示（後ほどLineRendererに変更?）
        Debug.DrawRay(ray.origin, ray.direction * rayLength, rayColor, 0, true);

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if (hit.collider.CompareTag("BeaconBullet") ||
                hit.collider.CompareTag("SnipeBullet") || hit.collider.CompareTag("Player") || hit.collider.CompareTag("Beacon")) return;
            effect.SetActive(true);
            effect.transform.rotation = Quaternion.LookRotation(hit.normal);
            effect.transform.position = hit.point;
        }
        else effect.SetActive(false);
    }

    /// <summary>
    /// ＊装備している武器の射撃
    /// </summary>
    void Fire(Ray ray)
    {
        //発射する武器の指定
        GameObject weapon = (isWeaponBeacon) ? Instantiate(beaconBullet) : Instantiate(snipeBullet);

        weapon.transform.position = ray.origin + transform.forward * 2; //発射位置の指定

        //装備している武器で発砲
        if (isWeaponBeacon)
            weapon.GetComponent<BeaconBullet>().Fire(ray.direction);
        else
            weapon.GetComponent<SniperBullet>().Fire(ray.direction);
    }

    /// <summary>
    /// ＊「ドローン」「ミサイル」をロックオンする
    /// </summary>
    void LockonTargets()
    {
        //ドローンとミサイルに対する距離をソートしたリスト
        var targets = aimRange.GetComponent<AimRange>().GetTarget();
        var targetCount = targets.Count;

        if (Input.GetButtonDown("LockAt")) targetNum++;
        if (targetNum >= targetCount) targetNum = 0;

        //Debug.Log("数：" + targetCount + "i：" + targetNum);

        if (targets.Count == 0) return;

        for (int i = 0; i < targetCount; i++)
        {
            if (targets.Values[i] == null)
            {
                targets.Clear();
                targetNum = 0;
                return;
            }

            if (targetNum == i)
            {
                Vector3 targetPos = targets.Values[i].transform.position;
                Vector3 distance = targetPos - transform.position;
                distance.y /= 1.5f;
                //Debug.Log(distance.sqrMagnitude);

                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(distance), lockonRotateSpeed);

                //sniper.GetComponent<Sniper>().HorizonAngle(distance, lockonRotateSpeed);
            }
        }
    }
}
