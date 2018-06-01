using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    XlinePhoto xline;

    [SerializeField] GameObject wheelManager;
    [SerializeField] WheelManager manager;

    [SerializeField] GameObject sniper;
    [SerializeField] GameObject driver;
    List<GameObject> soldiers = new List<GameObject>();

    [SerializeField] Transform endSEPoint; //SE終了ポイント

    [SerializeField, Range(0.1f, 10f)] float speed = 2f; //移動速度
    [SerializeField, Range(1, 500f)] float power = 20; //制動力（移動に影響）
    [SerializeField, Range(0.1f, 10f)] float rotate = 2; //回転量
    [SerializeField, Range(0.1f, 10f)] float driftRotate = 3f; //ドリフト時の回転量
    [SerializeField, Range(1, 5)] float brake_power = 5; //ブレーキ時反動
    [SerializeField] int residue = 3; //死亡可能回数

    //リスポーン用
    Vector3 startPosition;
    Quaternion startRotation;

    Rigidbody rb;

    //運転操作用
    float axel;
    float curve;

    bool _isEndSE = false; //開始演出の終了フラグ
    bool _isBrake = false; //ブレーキフラグ
    bool _isMove = true; //開始演出運転フラグ

    bool _isDead = false;
    Fade fade;

    void Start()
    {
        xline = canvas.GetComponent<XlinePhoto>();
        rb = gameObject.GetComponent<Rigidbody>();

        manager = wheelManager.GetComponent<WheelManager>();

        //リスポーン用初期情報
        startPosition = gameObject.transform.position;
        startRotation = gameObject.transform.rotation;

        soldiers.Add(sniper);
        soldiers.Add(driver);

        fade = GetComponent<Fade>();
    }

    void Update()
    {
        SEMove_X();

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
        if (other.collider.CompareTag("BigEnemy")) Respawn();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile")) Respawn();
    }

    /// <summary>
    /// 運転操作入力の取得
    /// </summary>
    void GetInputDrive()
    {
        //axel = Input.GetAxis("Axel");
        //curve = Input.GetAxis("Curve");
    }

    /// <summary>
    /// 運転
    /// </summary>
    void Drive()
    {
        //移動計算用
        //Vector3 move = manager.GetForward() * axel * speed;
        Vector3 move = rb.transform.forward * axel * speed;
        Vector3 force = power * move - rb.velocity;

        rb.AddForce(force); //移動

        var rotation = (axel == 0) ? driftRotate : rotate; //旋回力の選択

        //動いていればカーブ可能
        //if (Mathf.Abs(force.x) > 0.05f)
        rb.transform.Rotate(new Vector3(0.0f, curve * rotation, 0.0f));
    }

    /// <summary>
    /// ＊初期位置にリスポーン
    /// </summary>
    public void Respawn()
    {
        if (Annihilation()) return;

        //フェード

        _isDead = true;

        //移動を殺す
        rb.velocity = Vector3.zero;

        if (_isDead && Input.GetButtonDown("Select"))
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
    /// 自身とSEカメラの位置情報をもとに自動運転を行う
    /// </summary>
    void SEMove()
    {
        if (transform.position.x != endSEPoint.position.x && transform.position.z != endSEPoint.position.z)
        {
            SEMove_X();
            SEMove_Z();
        }
        else if (transform.position.x != endSEPoint.position.x) SEMove_X();
        else if (transform.position.z != endSEPoint.position.z) SEMove_Z();

        if (!_isEndSE) transform.LookAt(endSEPoint);
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

        //ブレーキ
        else Brake();

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
        Vector3 move = new Vector3(0, 0, endSEPoint.position.z - transform.position.z);

        //移動（目的地に近づくほど減速）
        if (_isMove) transform.position += new Vector3(0, 0, move.normalized.z / 10 + move.z / 100);

        //ブレーキ
        else Brake();

        //到着
        if (Mathf.Abs(move.z) < 1) _isMove = false;
    }

    /// <summary>
    /// ブレーキ時
    /// </summary>
    void Brake()
    {
        foreach (var soldier in soldiers)
        {
            //ブレーキ反動の力
            float brake = brake_power - soldier.transform.eulerAngles.x / 10;

            //元の位置に戻ろうとする
            if (_isBrake)
            {
                if (soldier.transform.eulerAngles.x >= 1)
                    soldier.transform.eulerAngles += new Vector3(-brake / brake_power, 0);
                else
                {
                    soldier.transform.eulerAngles = new Vector3(0, soldier.transform.eulerAngles.y);
                    _isEndSE = true;
                }
            }

            //ブレーキ反動処理
            else soldier.transform.eulerAngles += new Vector3(brake, 0);

            //ブレーキ反動の終了
            if (brake <= 0.2f) _isBrake = true;
        }
    }

    /// <summary>
    /// 開始演出が終わったか
    /// </summary>
    public bool GetIsEndSE()
    {
        return _isEndSE;
    }
}