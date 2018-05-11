using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //必須ゲームオブジェクト
    GameObject player;
    [SerializeField] GameObject cameraCon;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject aimRange;
    [SerializeField] GameObject beaconBullet;
    [SerializeField] GameObject snipeBullet;
    [SerializeField] GameObject blueRippel;
    [SerializeField] GameObject redRippel;
    [SerializeField] GameObject sniper;
    [SerializeField] GameObject laserSight;
    [SerializeField] GameObject lineObject;
    LineRenderer line;

    [SerializeField, Range(0.1f, 1f)] float homingSpeed = 1; //追従速度
    [SerializeField, Range(50, 500)] float angleRotateSpeed = 100; //手動アングル速度
    [SerializeField, Range(1, 500)] float fpsRotateSpeed = 50; //FPS時アングル速度
    [SerializeField, Range(0.1f, 10f)] float autoLookSpeed = 1; //自動修正アングル速度
    [SerializeField, Range(0.1f, 10f)] float lockonRotateSpeed = 10; //振り向き速度

    //上下アングルの制限範囲
    [SerializeField, Range(-90, 0)] float lowAngleLimit = -30;
    [SerializeField, Range(0, 90)] float highAngleLimit = 80;

    [SerializeField, Range(-90, 0)] float hideAngle = -30;

    //レーザーポインターの幅
    [SerializeField, Range(0.1f, 1)] float laserWide = 0.1f;
    //着弾点エフェクトを浮かす値
    [SerializeField, Range(0.1f, 3)] float effectPos = 1;
    //スナイパーライフルのクールタイム
    [SerializeField, Range(0.1f, 2)] float snipeCoolTime = 1;
    float backupCoolTime;

    Transform car; //プレイヤーのトランスフォーム

    bool isLockAt_Big; //ビッグエネミーLook用
    bool isBackAngle; //カメラの前後切替フラグ
    bool isSnipe; //射撃モードのオンオフ
    bool isWeaponBeacon; //武器の切替用

    bool isLockon;
    bool isLockonEnd;
    int targetNum;

    bool isLaserHit;
    float laserLength;

    bool isSnipeFire;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        line = lineObject.GetComponent<LineRenderer>();

        car = player.transform;
        isLockAt_Big = false;
        isBackAngle = false;
        isSnipe = false;
        isWeaponBeacon = true;
        isLockon = false;
        isLockonEnd = false;
        isLaserHit = false;
        isSnipeFire = true;

        targetNum = -1;

        backupCoolTime = snipeCoolTime;
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

        PlayerLoss();
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
        else if ((Input.GetButtonUp("BackAngle") && isBackAngle) || Input.GetButtonDown("CameraReset"))
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
        laserSight.transform.eulerAngles += new Vector3(angle.y * changer, angle.x) * rotateSpeed * Time.deltaTime;

        //-180＜上下の動き＜180に変更
        float angleX = 180 <= laserSight.transform.eulerAngles.x ?
            laserSight.transform.eulerAngles.x - 360 : laserSight.transform.eulerAngles.x;

        //更に上下の動きの制限
        laserSight.transform.eulerAngles =
            new Vector3(Mathf.Clamp(angleX, downLimit, upLimit), laserSight.transform.eulerAngles.y, laserSight.transform.eulerAngles.z);

        //プレイヤーの透過
        bool isHide = false;
        if (angleX <= hideAngle) isHide = true;
        mainCamera.GetComponent<MainCamera>().PlayerHide(isHide);

        SniperRotate(fps);
    }

    void SniperRotate(bool fps)
    {
        //メインカメラとFPSの時の変換処理
        float rotateSpeed = angleRotateSpeed;
        float downLimit = lowAngleLimit;
        float upLimit = highAngleLimit;
        if (fps)
        {
            rotateSpeed = fpsRotateSpeed;
            //downLimit = -highAngleLimit;
            //upLimit = -lowAngleLimit;
        }

        //右パッドの登録
        Vector3 angle =
            new Vector3(Input.GetAxis("CameraHorizontal"), Input.GetAxis("CameraVertical"), 0);

        //上下左右360度回転（更に下のコードで上下の動きを制限）
        sniper.transform.eulerAngles += new Vector3(0, angle.x) * rotateSpeed * Time.deltaTime;
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
        if (Input.GetButtonDown("LockAt"))
        {
            isLockon = true;
            isLockonEnd = false;
        }

        //FPS視点の切替（LB）
        if (Input.GetButtonDown("Shooting"))
        {
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
        Ray ray = new Ray(laserSight.transform.position + laserSight.transform.forward * 2, laserSight.transform.forward);

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

        bool isFire = (isWeaponBeacon) ? true : isSnipeFire;

        if (!isSnipeFire)
        {
            snipeCoolTime -= Time.deltaTime;
            if (snipeCoolTime <= 0)
            {
                isSnipeFire = true;
                snipeCoolTime = backupCoolTime;
            }
        }

        if (Input.GetButtonDown("Fire") && isFire) Fire(ray); //射撃

        if (!isLaserHit) laserLength = rayLength;

        DrawLine(ray.origin, ray.origin + ray.direction * laserLength, rayColor, laserWide);

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            //弾・ビーコン・プレイヤー・スナイパーらとの衝突は無視
            if (hit.collider.CompareTag("BeaconBullet") || hit.collider.CompareTag("SnipeBullet") ||
                hit.collider.CompareTag("Player") || hit.collider.CompareTag("Beacon") ||
                hit.collider.CompareTag("Sniper")) return;

            effect.SetActive(true);
            effect.transform.rotation = Quaternion.LookRotation(hit.normal);
            effect.transform.position = hit.point + hit.normal * effectPos;

            isLaserHit = true;
            if (isLaserHit) laserLength = Vector3.Distance(hit.point, ray.origin);
        }
        else
        {
            isLaserHit = false;
            effect.SetActive(false);
        }
    }

    /// <summary>
    /// ＊装備している武器の射撃
    /// </summary>
    void Fire(Ray ray)
    {
        //ビーコン重複防止処理
        if (isWeaponBeacon)
        {
            var beforBeacon = GameObject.FindGameObjectWithTag("Beacon");
            var beforBullet = GameObject.FindGameObjectWithTag("BeaconBullet");
            if (beforBeacon != null) Destroy(beforBeacon);
            if (beforBullet != null) Destroy(beforBullet);
        }

        //発射する武器の指定
        GameObject weapon = (isWeaponBeacon) ? Instantiate(beaconBullet) : Instantiate(snipeBullet);

        weapon.transform.position = ray.origin + laserSight.transform.forward; //発射位置の指定

        //装備している武器で発砲
        if (isWeaponBeacon)
            weapon.GetComponent<BeaconBullet>().Fire(ray.direction);
        else
        {
            weapon.GetComponent<SniperBullet>().Fire(ray.direction);
            isSnipeFire = false;
        }
    }

    /// <summary>
    /// ＊「ドローン」「ミサイル」をロックオンする
    /// </summary>
    void LockonTargets()
    {
        //ドローンとミサイルに対する距離をソートしたリスト
        var targets = aimRange.GetComponent<AimRange>().GetTarget();
        var targetCount = targets.Count;

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

            if (targetNum == i)
            {
                Vector3 targetPos = targets.Values[i].transform.position;
                Vector3 distance = targetPos - laserSight.transform.position;
                distance.y /= 1.5f;

                laserSight.transform.rotation =
                    Quaternion.RotateTowards(laserSight.transform.rotation, Quaternion.LookRotation(distance), lockonRotateSpeed);
            }

            if (targetNum == i)
            {
                Vector3 targetPos = targets.Values[i].transform.position;
                Vector3 distance = targetPos - sniper.transform.position;
                distance.y = 0;

                sniper.transform.rotation =
                    Quaternion.RotateTowards(sniper.transform.rotation, Quaternion.LookRotation(distance), lockonRotateSpeed);
            }
        }
    }

    void DrawLine(Vector3 p1, Vector3 p2, Color c1, float width)
    {
        line.SetPosition(0, p1);
        line.SetPosition(1, p2);
        line.startColor = c1;
        line.endColor = c1;
        line.startWidth = width;
        line.endWidth = width;
    }

    /// <summary>
    /// ＊プレイヤーとカメラの表示を消す
    /// </summary>
    public void Hide()
    {
        player.SetActive(false);
        cameraCon.SetActive(false);
    }

    void PlayerLoss()
    {
        if (player == null) cameraCon.SetActive(false);
    }
}
