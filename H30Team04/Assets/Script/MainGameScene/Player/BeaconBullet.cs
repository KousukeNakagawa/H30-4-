using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconBullet : MonoBehaviour
{
    /// <summary> 着弾音 </summary>
    [SerializeField] AudioClip clingSE;
    /// <summary> 弾速 </summary>
    [SerializeField] [Range(0, 300)] float speed = 50;

    /// <summary> 音 </summary>
    AudioSource audioSourse;
    /// <summary> リジッドボディ </summary>
    Rigidbody rb;
    /// <summary> 発射位置 </summary>
    Vector3 origin_;

    /// <summary> 光玉 </summary>
    GameObject m_Sphere;
    /// <summary> レンダラー </summary>
    Renderer _renderer;

    /// <summary> 射程距離 </summary>
    public static float RangeDistance_ { get { return 50; } }

    void Start()
    {
        // コンポーネント
        rb = GetComponent<Rigidbody>();
        audioSourse = GetComponent<AudioSource>();
        // 発射位置の取得
        origin_ = rb.position;

        // 光玉の検索・取得
        if (GameObject.Find("Sphere") != null)
            m_Sphere = GameObject.Find("Sphere").gameObject;
        m_Sphere.SetActive(false);

        // 光玉の設定
        _renderer = transform.Find("Sphere").gameObject.GetComponent<Renderer>();
        _renderer.material.EnableKeyword("_EMISSION"); //キーワードの有効化を忘れずに
        _renderer.material.SetColor("_EmissionColor", new Color(50, 0, 0)); //赤色に光らせたい
    }

    void Update()
    {
        // 射程距離消滅
        OverRange();
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
        else Destroy(gameObject);
    }

    /// <summary> 射程外消滅 </summary>
    void OverRange()
    {
        // 飛距離
        Vector3 FlyDistance = rb.position - origin_;

        // 飛距離が射程距離を超えたら消滅
        if (FlyDistance.magnitude > RangeDistance_) Destroy(gameObject);
    }

    /// <summary> ビーコンの発射 </summary>
    public void Fire(Vector3 direction)
    {
        // 一度のみ
        Start();
        // 一時停止中は速度０
        var speed = (Time.timeScale == 0) ? 0 : this.speed;
        rb.velocity = direction * speed;
    }

    /// <summary> 着弾 </summary>
    void Cling(Collision other, bool isField = true)
    {
        // 位置固定
        transform.position = WeaponCtrl.BeaconHitPos;
        // 角度を変更
        transform.rotation = WeaponCtrl.BeaconAngle(isField);
        // 動きを止める
        rb.constraints = RigidbodyConstraints.FreezeAll;
        //当たったオブジェクトの子になる
        transform.parent = other.transform;
        //tagを「BeaconBullet」から「Beacon」へ
        transform.tag = "Beacon";
        // 音を鳴らす
        audioSourse.PlayOneShot(clingSE);

        // 光玉の再検索
        m_Sphere = m_Sphere ?? GameObject.Find("Sphere").gameObject;
        m_Sphere.SetActive(true);
    }

    /// <summary> 弾速 </summary>
    public static float GetRangeDistance()
    {
        return RangeDistance_;
    }

    /// <summary> 赤く光らせる </summary>
    void OnTriggerStay(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Ignore Raycast")
        {
            _renderer.material.SetColor("_EmissionColor", new Color(0, 17, 50)); //赤色に光らせたい
        }
    }
}
