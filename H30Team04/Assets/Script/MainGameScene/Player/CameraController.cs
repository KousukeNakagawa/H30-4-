﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //必須ゲームオブジェクト
    GameObject player;
    [SerializeField] GameObject beaconBullet;
    [SerializeField] GameObject snipeBullet;
    [SerializeField] GameObject blueRippel;
    [SerializeField] GameObject redRippel;
    [SerializeField] GameObject laserSight;
    [SerializeField] GameObject lineObject;
    LineRenderer line;

    //ロックオン速度
    [SerializeField, Range(0.1f, 10f)] float lockonRotateSpeed = 10;
    //レーザーポインターの幅
    [SerializeField, Range(0.1f, 1)] float laserWide = 0.1f;
    //着弾点エフェクトを浮かす値
    [SerializeField, Range(0.1f, 3)] float effectPos = 1;
    //スナイパーライフルのクールタイム
    [SerializeField, Range(0.1f, 2)] float snipeCoolTime = 1;
    float backupCoolTime;

    bool isWeaponBeacon; //武器の切替用

    bool isLaserHit;
    float laserLength;

    bool isSnipeFire;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        line = lineObject.GetComponent<LineRenderer>();

        isWeaponBeacon = true;
        isLaserHit = false;
        isSnipeFire = true;

        backupCoolTime = snipeCoolTime;
    }

    void Update()
    {
        SetChanger(); //カメラモード切替

        ShootingMode();
    }

    /// <summary>
    /// ＊プレイヤーとカメラの表示を消す
    /// </summary>
    public void Hide()
    {
        player.SetActive(false);
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

        LaserPointer(ray.origin, ray.origin + ray.direction * laserLength, rayColor, laserWide);

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

    void LaserPointer(Vector3 p1, Vector3 p2, Color c1, float width)
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
    /// <returns></returns>
    public bool GetWeapon()
    {
        return isWeaponBeacon;
    }
}
