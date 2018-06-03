using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject rifle;

    //走る速度
    [SerializeField, Range(10, 500)] float speed = 100;

    Rigidbody rb;

    //操作入力取得用
    float Hor;
    float Ver;

    bool isWeaponBeacon = true;

    //死亡可能回数
    [SerializeField] int residue = 3;

    //リスポーン用
    Vector3 startPosition;
    Quaternion startRotation;

    //死亡判定
    //bool _isDead = false;

    public static bool IsDead { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        IsDead = false;
    }

    void Update()
    {
        if (!SEManager.IsEndSE) return;

        DirectionSet();

        GetInput();

        if (Annihilation()) Death(); //３回やられたら死亡

        //START:初期位置へワープ（デバック用）
        if (Input.GetButtonDown("Restart")) IsDead = true;

        Respawn();
    }

    void FixedUpdate()
    {
        Move();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("BigEnemy")) Respawn();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile")) Respawn();
    }

    /// <summary> 移動処理 </summary>
    void Move()
    {
        if (!UnlockManager.limit[TutorialState.move]) return;

        var move = ((transform.forward * Ver) + (transform.right * Hor)).normalized;
        rb.velocity = move * speed * Time.deltaTime;
    }

    /// <summary> カメラと向きを合わせる </summary>
    void DirectionSet()
    {
        //カメラの向きの取得
        var dir = playerCamera.transform.forward;

        rifle.transform.forward = dir;

        //プレイヤーは上下アングルを無視する
        dir.y = 0;
        transform.forward = dir;
    }

    /// <summary> 操作入力の取得 </summary>
    void GetInput()
    {
        Hor = Input.GetAxis("Hor");
        Ver = Input.GetAxis("Ver");
    }

    /// <summary> 装備中の武器の取得 </summary>
    public bool GetWeapon()
    {
        return isWeaponBeacon;
    }

    /// <summary> リスポーン </summary>
    public void Respawn()
    {
        if (Annihilation()) return;

        if (IsDead)
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
                IsDead = false;
            }
        }
    }

    /// <summary> 残機０で死亡時 </summary>
    public bool Annihilation()
    {
        return (residue <= 0) ? true : false;
    }

    /// <summary> 死亡処理 </summary>
    void Death()
    {
        //カメラは破壊しない
        Camera.main.transform.parent = null;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }
}