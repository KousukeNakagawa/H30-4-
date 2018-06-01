using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] float speed = 100;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject rifle;
    float moveX;
    float moveZ;
    Rigidbody rb;



    [SerializeField] GameObject beaconBullet;
    [SerializeField] GameObject snipeBullet;
    [SerializeField] GameObject blueRippel;
    [SerializeField] GameObject redRippel;
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



    //開始演出終了ポイント
    [SerializeField] Transform endSEPoint;
    //通常速度
    [SerializeField, Range(0.1f, 10f)] float walkSpeed = 2f;
    //ダッシュ時速度
    [SerializeField, Range(0.1f, 100f)] float dashSpeed = 6f;
    //旋回力
    [SerializeField, Range(0.1f, 30f)] float rotateSpeed = 2;
    //死亡可能回数
    [SerializeField] int residue = 3;


    //リスポーン用
    Vector3 startPosition;
    Quaternion startRotation;

    bool _isEndSE = false; //開始演出の終了フラグ
    bool _isMove = true; //開始演出運転フラグ
    //死亡判定
    bool _isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        line = laser.GetComponent<LineRenderer>();
    }

    void Update()
    {
        //カメラの向きとプレイヤーの向きを統一
        var dir = playerCamera.transform.forward;
        rifle.transform.forward = dir;
        dir.y = 0;
        transform.forward = dir;

        GetInput();

        ShootingMode();
    }

    void GetInput()
    {
        moveX = Input.GetAxis("Hor");
        moveZ = Input.GetAxis("Ver");
    }

    void FixedUpdate()
    {
        var move = ((transform.forward * moveZ) + (transform.right * moveX)).normalized;
        rb.velocity = move * speed * Time.deltaTime;
    }

    /// <summary>
    /// ＊射撃モード
    /// </summary>
    void ShootingMode()
    {
        //武器の切替
        if (Input.GetButtonDown("WeaponChange")) isWeaponBeacon = !isWeaponBeacon;

        RaycastHit hit; //レイとの衝突場所にエフェクトを後々追加予定

        //自分の位置の少し上から正面へ（微調整）
        Ray ray = new Ray(rifle.transform.position + rifle.transform.forward * 2, rifle.transform.forward);

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

        weapon.transform.position = ray.origin + rifle.transform.forward; //発射位置の指定

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

    /// <summary>
    /// リスポーン
    /// </summary>
    public void Respawn()
    {
        if (Annihilation()) return;

        if (_isDead)
        {
            //移動を殺す
            rb.velocity = Vector3.zero;

            if (Input.GetButtonDown("Select"))
            {
                //初期位置へ
                transform.position = startPosition;
                transform.rotation = startRotation;
                //ビッグエネミーを向く
                transform.LookAt(BigEnemyScripts.mTransform);
                residue--; //死亡可能回数の減少
                _isDead = false;
            }
        }
    }

    /// <summary>
    /// 死亡判定
    /// </summary>
    public bool IsDead()
    {
        return _isDead;
    }

    /// <summary>
    /// ＊残機が0になったら「全滅」＝True
    /// </summary>
    public bool Annihilation()
    {
        return (residue <= 0) ? true : false;
    }

    /// <summary>
    /// ＊死亡
    /// </summary>
    void Death()
    {
        //カメラは破壊しない
        Camera.main.transform.parent = null;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }

    /// <summary>
    /// 開始演出の自動運転（X軸）
    /// </summary>
    void SEMove_X()
    {
        if (_isEndSE) return;

        //移動量
        Vector3 move = new Vector3(endSEPoint.position.x - transform.position.x, 0);

        //移動（目的地に近づくほど減速）
        if (_isMove) transform.position += new Vector3(move.normalized.x / 10 + move.x / 100, 0);

        //開始演出の終了
        else _isEndSE = true;

        //到着
        if (Mathf.Abs(move.x) < 1) _isMove = false;
    }

    /// <summary>
    /// 開始演出の自動運転（Z軸）
    /// </summary>
    void SEMove_Z()
    {
        if (_isEndSE) return;

        //移動量
        Vector3 move = new Vector3(endSEPoint.position.z - transform.position.z, 0);

        //移動（目的地に近づくほど減速）
        if (_isMove) transform.position += new Vector3(move.normalized.z / 10 + move.z / 100, 0);

        //開始演出の終了
        else _isEndSE = true;

        //到着
        if (Mathf.Abs(move.z) < 1) _isMove = false;
    }

    /// <summary>
    /// 開始演出が終わったか
    /// </summary>
    public bool GetIsEndSE()
    {
        return _isEndSE;
    }
}
