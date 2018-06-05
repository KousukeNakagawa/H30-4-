using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject beaconBullet;
    [SerializeField] GameObject snipeBullet;
    [SerializeField] GameObject rippel;
    [SerializeField] GameObject laser;
    LineRenderer line;

    //レーザーポインターの幅
    [SerializeField, Range(0.1f, 1)] float laserWide = 0.1f;

    //着弾点エフェクトを浮かす値
    [SerializeField, Range(0, 3)] float effectPos = 0.1f;
    //スナイパーライフルのクールタイム
    [SerializeField, Range(0, 3)] float snipeCoolTime = 1;
    float backupCoolTime;

    bool isSnipeFire = true;
    bool isLaserHit = false;
    float laserLength;

    /// /// <summary> 装備中の武器（true = beacon / false = snipe） </summary>
    public static bool WeaponBeacon { get; private set; }

    void Start()
    {
        //初期装備はビーコン
        WeaponBeacon = true;
        line = laser.GetComponent<LineRenderer>();
    }

    void Update()
    {
        ChangeWeapon();
        Shooting();
    }

    void ChangeWeapon()
    {
        //武器の切替
        if (Input.GetButtonDown("WeaponChange")) WeaponBeacon = !WeaponBeacon;
    }

    /// <summary> レーザーポインターの描画 </summary>
    void Shooting()
    {
        //発射位置・方向
        var ray = new Ray(muzzle.position, muzzle.forward);

        //武器によって長さが変わる
        var rayLength = (WeaponBeacon) ?
            BeaconBullet.GetRangeDistance() : SniperBullet.GetRangeDistance();

        //武器によって色が変わる
        var rayColor = (WeaponBeacon) ? Color.blue : Color.red;

        //クールタイム
        var isFire = (WeaponBeacon) ? true : isSnipeFire;

        //クールタイム処置
        if (!isSnipeFire)
        {
            snipeCoolTime -= Time.deltaTime;
            if (snipeCoolTime <= 0)
            {
                isSnipeFire = true;
                snipeCoolTime = backupCoolTime;
            }
        }

        //射撃可能なら射撃
        if (Input.GetButtonDown("Fire") && isFire) Fire(ray);

        //レイの衝突判定用
        RaycastHit hit;

        //レーザーの長さ
        if (!isLaserHit) laserLength = rayLength;

        LaserPointer(ray.origin, ray.direction * laserLength, rayColor, laserWide);

        //レイが何かに衝突したら
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            //弾・ビーコン・プレイヤー・スナイパーらとの衝突は無視
            if (hit.collider.CompareTag("BeaconBullet") || hit.collider.CompareTag("SnipeBullet") ||
                hit.collider.CompareTag("Player") || hit.collider.CompareTag("Beacon") ||
                hit.collider.CompareTag("Sniper") || hit.collider.CompareTag("GoalPoint")) return;

            rippel.SetActive(true);
            rippel.transform.rotation = Quaternion.LookRotation(hit.normal);
            rippel.transform.position = hit.point + hit.normal * effectPos;

            isLaserHit = true;
            if (isLaserHit) laserLength = Vector3.Distance(hit.point, ray.origin);
        }
        else
        {
            isLaserHit = false;
            rippel.SetActive(false);
        }
    }

    /// <summary> 装備中の武器の使用 </summary>
    void Fire(Ray ray)
    {
        //他のビーコンを削除
        if (WeaponBeacon) OtherBeaconDedtroy();

        //発射する武器の指定
        var weapon = (WeaponBeacon) ? Instantiate(beaconBullet) : Instantiate(snipeBullet);

        //発射位置
        weapon.transform.position = ray.origin + muzzle.forward;

        //装備している武器で射撃
        if (WeaponBeacon) weapon.GetComponent<BeaconBullet>().Fire(ray.direction);
        else
        {
            weapon.GetComponent<SniperBullet>().Fire(ray.direction);
            isSnipeFire = false;
        }
    }

    /// <summary> ビーコン重複防止 </summary>
    void OtherBeaconDedtroy()
    {
        var beforBeacon = GameObject.FindGameObjectWithTag("Beacon");
        var beforBullet = GameObject.FindGameObjectWithTag("BeaconBullet");
        if (beforBeacon != null) Destroy(beforBeacon);
        if (beforBullet != null) Destroy(beforBullet);
    }

    /// <summary> 予測線の描画 </summary>
    void LaserPointer(Vector3 p1, Vector3 p2, Color c1, float width)
    {
        line.SetPosition(0, p1);
        line.SetPosition(1, p1 + p2);
        line.startColor = c1;
        line.endColor = c1;
        line.startWidth = width;
        line.endWidth = width;
    }
}