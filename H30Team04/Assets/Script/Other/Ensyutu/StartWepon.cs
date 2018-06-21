using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWepon : MonoBehaviour {

    [SerializeField] GameObject beaconGun;
    [SerializeField] GameObject beaconBullet;
    [SerializeField] GameObject rippel;
    [SerializeField] GameObject laser;
    [SerializeField] Transform beaconPos;
    LineRenderer line;

    //レーザーポインターの幅
    [SerializeField, Range(0.01f, 0.1f)] float laserWide = 0.1f;

    //着弾点エフェクトを浮かす値
    [SerializeField, Range(0, 3)] float effectPos = 0.1f;
    
    bool isLaserHit = false;
    float laserLength;

    bool laserSet = false;

    AudioSource audioSourse;
    

    void Start()
    {
        line = laser.GetComponent<LineRenderer>();

        audioSourse = gameObject.AddComponent<AudioSource>();
        audioSourse.volume = 0.2f;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (!UnlockManager.Limiter[UnlockState.move]) return;
        

        //if (!laser.activeSelf) laser.SetActive(true);
        
    }

    void LateUpdate()
    {
        Shooting();
        //laser.SetActive(UnlockManager.Limiter[UnlockState.laserPointer]);
    }
    

    /// <summary> レーザーポインターの描画 </summary>
    void Shooting()
    {

        var rayMuzzle =  beaconGun.transform.Find("BeaconMuzzle");

        //発射位置・方向
        var ray = new Ray(rayMuzzle.position, Vector3.Normalize(beaconPos.position - rayMuzzle.position));

        //武器によって長さが変わる
        var rayLength = 200;

        //武器によって色が変わる
        var rayColor = Color.blue;


        var fireMuzzle = rayMuzzle;

        //射撃可能なら射撃
        //Fire(ray, fireMuzzle);

        //レイの衝突判定用
        RaycastHit hit;

        //レーザーの長さ
        if (!isLaserHit) laserLength = rayLength;

        if (laserSet) LaserPointer(ray.origin, ray.direction * laserLength, rayColor, laserWide);
        else laser.SetActive(false);
        
        //レイが何かに衝突したら
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            //弾・ビーコン・プレイヤー・スナイパーらとの衝突は無視
            if (hit.collider.CompareTag("BeaconBullet") || hit.collider.CompareTag("SnipeBullet") ||
                hit.collider.CompareTag("Player") ||
                hit.collider.CompareTag("Sniper") /*|| hit.collider.CompareTag("GoalPoin")*/) return;
            
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
    void Fire(Ray ray, Transform muzzle)
    {
        //他のビーコンを削除
        OtherBeaconDedtroy();

        //発射する武器の指定
        var weapon = Instantiate(beaconBullet);

        //発射位置
        weapon.transform.position = muzzle.position - Vector3.up * 0.2f + Vector3.forward * 0.2f;

        //装備している武器で射撃

        //GameTextController.TextStart(1);
        weapon.GetComponent<BeaconBullet>().Fire(ray.direction);
        audioSourse.Play();

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
        laser.SetActive(true);
        line.SetPosition(0, p1);
        line.SetPosition(1, p1 + p2);
        line.startColor = c1;
        line.endColor = c1;
        line.startWidth = width;
        line.endWidth = width;
    }
    

    public void ChengeLaser()
    {
        laserSet = !laserSet;
    }
}
