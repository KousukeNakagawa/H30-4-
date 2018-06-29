using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconBullet : MonoBehaviour
{
    [SerializeField] GameObject beacon;
    [SerializeField] [Range(0, 300)] float speed = 50; //弾速
    [SerializeField] [Range(5, 100)] static float rangeDistance = 50; //射程距離
    AudioSource audioSourse;
    [SerializeField] AudioClip SE;

    Rigidbody rb;
    Vector3 startPos; //初期位置
    GameObject m_Sphere;
    public bool IsChange { get; private set; } //タグ変化把握用

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSourse = GetComponent<AudioSource>();
        startPos = rb.position;
        IsChange = false;
        if (GameObject.Find("Sphere") != null)
            m_Sphere = GameObject.Find("Sphere").gameObject;
        m_Sphere.SetActive(false);
    }

    void Update()
    {
        OverRange(); //射程外消滅処理
    }

    void OnCollisionEnter(Collision other)
    {
        //プレイヤー・スナイパーらとの衝突は無視
        if (other.collider.CompareTag("Player") || other.collider.CompareTag("Sniper") ||
            other.collider.CompareTag("BeaconBullet") || other.collider.CompareTag("Beacon")) return;

        //ビル・地面に衝突時、くっつく
        else if (other.collider.CompareTag("Building")) Cling(other, false);
        else if (other.collider.CompareTag("Field")) Cling(other);

        //上記以外と衝突時、自身を破壊する
        else Destroy(beacon);
    }

    /// <summary>
    /// 射程外消滅処理
    /// </summary>
    void OverRange()
    {
        //飛距離
        Vector3 FlyDistance = rb.position - startPos;

        //飛距離が射程距離を超えたら消滅
        if (FlyDistance.magnitude > rangeDistance) Destroy(beacon);
    }

    /// <summary>
    /// ビーコンの発射
    /// </summary>
    public void Fire(Vector3 direction)
    {
        Start();
        var speed = (Time.timeScale == 0) ? 0 : this.speed;
        rb.velocity = direction * speed;
    }

    /// <summary>
    /// 張り付き処理
    /// </summary>
    void Cling(Collision other, bool isField = true)
    {
        // 位置固定
        transform.position = WeaponCtrl.HitPos;
        // 角度を変更
        transform.rotation = WeaponCtrl.BeaconAngle(isField);
        // 動きを止める
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //当たったオブジェクトの子になる
        transform.parent = other.transform;
        //tagを「BeaconBullet」から「Beacon」へ
        transform.tag = "Beacon";
        IsChange = true;
        // 音を鳴らす
        audioSourse.PlayOneShot(SE);

        m_Sphere = m_Sphere ?? GameObject.Find("Sphere").gameObject;
        m_Sphere.SetActive(true);
    }

    /// <summary>
    /// 射程距離のゲッター
    /// </summary>
    public static float GetRangeDistance()
    {
        return rangeDistance;
    }

    /// <summary>
    /// ビーコン・ビーコンバレットが一つもなければ発射許可
    /// </summary>
    //public bool IsFireOK()
    //{
    //    return (!GameObject.FindGameObjectWithTag("Beacon") &&
    //        !GameObject.FindGameObjectWithTag("BeaconBullet")) ? true : false;
    //}
}
