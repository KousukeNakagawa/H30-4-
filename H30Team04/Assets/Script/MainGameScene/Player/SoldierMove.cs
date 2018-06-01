using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMove : MonoBehaviour
{
    //構える武器
    [SerializeField] GameObject rifle;
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

    Rigidbody rb;

    //リスポーン用
    Vector3 startPosition;
    Quaternion startRotation;

    //操作入力の取得用
    float Hor;
    float Ver;

    bool _isEndSE = false; //開始演出の終了フラグ
    bool _isMove = true; //開始演出運転フラグ
    //死亡判定
    bool _isDead = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        //リスポーン用初期情報
        startPosition = gameObject.transform.position;
        startRotation = gameObject.transform.rotation;
    }

    void Update()
    {
        //SEMove_Z();
        ////SEMove_X();

        //if (!_isEndSE) return;

        GetMoveInput();

        if (Annihilation()) Death(); //３回やられたら死亡

        //START:初期位置へワープ（デバック用）
        if (Input.GetButtonDown("Restart")) _isDead = true;

        Respawn();
    }

    void FixedUpdate()
    {
        Walk();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("BigEnemy")) Respawn();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile")) Respawn();
    }

    /// <summary>
    /// 移動操作の取得
    /// </summary>
    void GetMoveInput()
    {
        Hor = Input.GetAxis("Hor");
        Ver = Input.GetAxis("Ver");
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Walk()
    {
        if (Camera.main == null) return;

        //カメラの方向から、単位ベクトルの取得
        Vector3 cameraForward =
            Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //移動方向の決定
        Vector3 moveForward = cameraForward * Ver + Camera.main.transform.right * Hor * Time.deltaTime;

        //移動方向に進む
        rb.velocity = moveForward * dashSpeed + new Vector3(0, rb.velocity.y);

        //向きを進行方向へ
        if (moveForward != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveForward);
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
