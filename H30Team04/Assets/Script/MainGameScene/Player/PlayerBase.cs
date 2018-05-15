using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    XlinePhoto xline;

    [SerializeField] GameObject sniper;

    [SerializeField] Transform endSEPoint; //SE終了ポイント

    [SerializeField, Range(0.1f, 10f)] float speed = 2f; //移動速度
    [SerializeField, Range(1, 500f)] float power = 20; //制動力（移動に影響）
    [SerializeField, Range(0.1f, 10f)] float rotate = 2; //回転量
    [SerializeField, Range(0.1f, 10f)] float driftRotate = 3f; //ドリフト時の回転量
    [SerializeField] int residue = 3; //死亡可能回数
    [SerializeField, Range(1, 5)] float brake_power = 5;

    //リスポーン用
    Vector3 startPosition;
    Quaternion startRotation;

    Rigidbody rb;

    //運転操作用
    float axel;
    float curve;

    bool _isEndSE = false; //開始演出の終了フラグ
    bool _isBrake = false;
    bool _isSE = true;

    void Start()
    {
        xline = canvas.GetComponent<XlinePhoto>();
        rb = gameObject.GetComponent<Rigidbody>();
        //リスポーン用初期情報
        startPosition = gameObject.transform.position;
        startRotation = gameObject.transform.rotation;
    }

    void Update()
    {
        StraightSEMove();

        if (!_isEndSE) return;

        GetInputDrive(); //運転操作入力の取得

        if (Annihilation()) Death(); //３回やられたら死亡

        //START:初期位置へワープ（デバック用）
        if (Input.GetButtonDown("Restart")) Respawn();
    }

    void FixedUpdate()
    {
        Drive(); //運転
    }

    void OnCollisionEnter(Collision other)
    {
        //敵に衝突時、死亡
        if (other.collider.CompareTag("BigEnemy") || other.collider.CompareTag("Missile")) Respawn();
    }

    /// <summary>
    /// ＊運転操作入力の取得
    /// </summary>
    void GetInputDrive()
    {
        axel = Input.GetAxis("Axel");
        curve = Input.GetAxis("Curve");
    }

    /// <summary>
    /// ＊運転操作
    /// </summary>
    void Drive()
    {
        //移動計算用
        Vector3 move = rb.transform.forward * axel * speed;
        Vector3 force = power * move - rb.velocity;

        rb.AddForce(force); //移動

        var rotation = (axel == 0) ? driftRotate : rotate; //旋回力の選択

        //動いていればカーブ可能
        if (Mathf.Abs(force.x) > 0.05f)
            rb.transform.Rotate(new Vector3(0.0f, curve * rotation, 0.0f));
    }

    /// <summary>
    /// ＊初期位置にリスポーン
    /// </summary>
    public void Respawn()
    {
        if (Annihilation()) return;
        //初期位置へ
        transform.position = startPosition;
        transform.rotation = startRotation;
        residue--; //死亡可能回数の減少
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
    /// 開始演出の自動運転(直進)
    /// </summary>
    void StraightSEMove()
    {
        if (_isEndSE) return;

        //移動量
        Vector3 move = new Vector3(endSEPoint.position.x - transform.position.x, 0);

        //移動（目的地に近づくほど減速）
        if (_isSE) transform.position += new Vector3(move.normalized.x / 10 + move.x / 100, 0);
        //ブレーキ
        else
        {
            Brake();
        }
        //到着
        if (move.x <= 0) _isSE = false;
    }

    /// <summary>
    /// ブレーキ時
    /// </summary>
    void Brake()
    {
        //ブレーキ反動の力
        float brake = brake_power - sniper.transform.eulerAngles.x / 10;
        //元の位置に戻ろうとする
        if (_isBrake)
        {
            if (sniper.transform.eulerAngles.x >= 1)
                sniper.transform.eulerAngles += new Vector3(-brake / brake_power, 0);
            else
            {
                sniper.transform.eulerAngles = new Vector3(0, sniper.transform.eulerAngles.y);
                _isEndSE = true;
            }
        }
        //ブレーキ反動処理
        else sniper.transform.eulerAngles += new Vector3(brake, 0);
        //ブレーキ反動の終了
        if (brake <= 0.2f) _isBrake = true;
    }

    /// <summary>
    /// 開始演出が終わったか
    /// </summary>
    public bool GetIsEndSE()
    {
        return _isEndSE;
    }
}