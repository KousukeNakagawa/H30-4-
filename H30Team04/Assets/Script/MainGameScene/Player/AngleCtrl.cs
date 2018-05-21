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
    [SerializeField] GameObject projection;
    FireCtrl fire;
    Transform car; //PlayerのTransform

    List<GameObject> snipers = new List<GameObject>(); //sniper rifle cameraをまとめる
    [SerializeField] GameObject cameraAndRifle;
    [SerializeField] GameObject sniper;
    [SerializeField] GameObject playerCamera;

    [SerializeField] GameObject captureRange;


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




    [SerializeField] GameObject beaconBullet;
    [SerializeField] GameObject snipeBullet;
    [SerializeField] GameObject blueRippel;
    [SerializeField] GameObject redRippel;
    [SerializeField] GameObject laserSight;
    [SerializeField] GameObject laser;
    LineRenderer line;
    //レーザーポインターの幅
    [SerializeField, Range(0.1f, 1)] float laserWide = 0.1f;
    //着弾点エフェクトを浮かす値
    [SerializeField, Range(0, 3)] float effectPos = 1;
    [SerializeField, Range(0, 3)] float snipeCoolTime = 1;
    float backupCoolTime;

    bool isSnipeFire = true;
    bool isWeaponBeacon = true;
    bool isLaserHit = false;
    float laserLength;


    bool _isLockon = false;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerBase>();
        fire = projection.GetComponent<FireCtrl>();
        car = player.transform;

        snipers.Add(cameraAndRifle);
        snipers.Add(sniper);




        line = laser.GetComponent<LineRenderer>();
        backupCoolTime = snipeCoolTime;
    }

    void Update()
    {
        if (!playerScript.GetIsEndSE()) return;

        //SetChanger();

        CameraAngleControl(); //カメラアングル

        //LockonTargets();

        LookBigEnemy();


        SetChanger();
        ShootingMode();
    }

    /// <summary>
    /// ＊カメラアングルのコントロール
    /// </summary>
    void CameraAngleControl()
    {
        CameraReturn();
        //if (Input.GetButton("BackAngle")) return; //後ろを向いている間は後ろに固定

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
                transform.localEulerAngles =
                    new Vector3(item.transform.localEulerAngles.x, item.transform.localEulerAngles.y + 180, item.transform.localEulerAngles.z);

            //すぐに正面を向く
            else if (Input.GetButtonUp("BackAngle") || Input.GetButtonDown("CameraReset"))
                transform.localEulerAngles =
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
            {
                item.transform.rotation =
                    Quaternion.RotateTowards(item.transform.rotation, car.rotation, lookFrontSpeed);
            }
    }

    void LookBigEnemy()
    {
        if (Input.GetButtonDown("LockAt")) _isLockon = !_isLockon;
        if (_isLockon)
            foreach (var item in snipers)
            {
                item.transform.LookAt(BigEnemyScripts.mTransform.position);
            }

        Debug.Log(_isLockon);
    }

    /// <summary>
    /// ビッグエネミー・ドローン・ミサイルをロックオン
    /// </summary>
    void LockonTargets()
    {
        //スナイパー装備時のみロックオン
        if (fire.GetWeapon()) return;

        //ドローンとミサイルに対する距離をソートしたリスト
        var targets = captureRange.GetComponent<AimRange>().GetTarget();
        //対象の数
        var targetCount = targets.Count;

        //ビッグエネミーの取得
        if (!targets.ContainsValue(BigEnemyScripts.mTransform.gameObject))
            targets.Add(Vector3.Distance(BigEnemyScripts.mTransform.position, transform.position), BigEnemyScripts.mTransform.gameObject);

        //Debug.Log("数：" + targetCount + "i：" + targetNum);

        //ロックオン対象がいないと何もしない
        if (targets.Count == 0) return;

        //対象の切替
        if (Input.GetButtonDown("LockAt")) targetNum++;
        if (targetNum >= targetCount) targetNum = 0;

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
            //isObstcleがtrueかつ間に障害物があればロックオン不可能
            if (!isObstcle)
                if (!isLookOK) return;

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



    /// <summary>
    /// ＊視点や武器の切替
    /// </summary>
    void SetChanger()
    {
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
    /// 装備中の武器（true = beacon / false = snipe）
    /// </summary>
    public bool GetWeapon()
    {
        return isWeaponBeacon;
    }
}
