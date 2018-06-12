using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;

    //走る速度
    [SerializeField, Range(10, 500)] float speed = 100;
    //カメラ回転速度
    [SerializeField, Range(1, 500)] int _rotateSpeed = 100;
    //isAim=true時のカメラ回転速度
    [SerializeField, Range(1, 100)] int aimRotateSpeed = 20;
    //上下角度範囲
    [SerializeField, Range(0, 80)] float maxAngle = 80;
    [SerializeField, Range(0, -80)] float minAngle = -25;
    //プレイヤーを透過する角度
    [SerializeField, Range(0, 90)] float hideAngle = 35;

    //カメラ反転のON/OFF
    [SerializeField] bool inverted = false;

    Rigidbody rb;

    //操作入力取得用
    float Hor;
    float Ver;
    Vector3 angle;

    bool isWeaponBeacon = true;

    //死亡可能回数
    [SerializeField] int residue = 3;

    [SerializeField] bool isUnlock = true;

    AudioSource audioSourse;
    [SerializeField] AudioClip SE;

    //リスポーン用
    Vector3 startPosition;
    Quaternion startRotation;
    Quaternion startCameraRotation;

    //死亡判定
    //bool _isDead = false;

    public static bool IsDead { get; private set; }
    public static bool IsStop { get; set; }

    /// <summary> 移動しているかどうか（アニメーション用） </summary>
    public static bool IsMove { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSourse = gameObject.AddComponent<AudioSource>();
        IsDead = false;
        IsStop = false;

        startPosition = transform.position;
        startRotation = transform.rotation;
        startCameraRotation = playerCamera.transform.rotation;

        UnlockManager.AllSet(isUnlock);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (!SEManager.IsEndSE) return;

        GetInput();
        Rotation();

        if (Annihilation()) Death(); //３回やられたら死亡

        //START:初期位置へワープ（デバック用）
        //if (Input.GetButtonDown("Restart")) IsDead = true;

        //Respawn();
    }

    void FixedUpdate()
    {
        if (IsStop) rb.velocity = Vector3.zero;

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
        if (!UnlockManager.limit[UnlockState.move]) return;

        var speed = (IsStop) ? 0 : this.speed;

        var move = ((transform.forward * Ver) + (transform.right * Hor)).normalized;
        rb.velocity = move * speed * Time.deltaTime;

        IsMove = (rb.velocity != Vector3.zero);
        //if (Ver != 0 || Hor != 0) audioSourse.PlayOneShot(SE);
    }

    /// <summary> 回転処理 </summary>
    void Rotation()
    {
        if (!UnlockManager.limit[UnlockState.move]) return;
        //カメラ反転の対応
        float changer = (inverted) ? 1 : -1;
        //AIM時の対応
        float rotateSpeed = (Input.GetButton("Shooting")) ? aimRotateSpeed : _rotateSpeed;

        //プレイヤーの回転
        transform.eulerAngles += new Vector3(0, angle.x) * rotateSpeed * Time.deltaTime;

        //カメラとライフルの回転
        playerCamera.transform.eulerAngles += new Vector3(angle.y * changer, angle.x) * rotateSpeed * Time.deltaTime;
        //playerCamera.transform.forward = transform.forward;

        //-180＜上下の動き＜180に変更
        float angleX = (180 <= playerCamera.transform.eulerAngles.x) ?
            playerCamera.transform.eulerAngles.x - 360 : playerCamera.transform.eulerAngles.x;

        //上下の制限
        playerCamera.transform.eulerAngles =
            new Vector3(Mathf.Clamp(angleX, -maxAngle, -minAngle), playerCamera.transform.eulerAngles.y, playerCamera.transform.eulerAngles.z);

        //playerCamera.transform.position = transform.position - transform.forward * 2 + Vector3.up * 1.5f;

        //プレイヤーの透過
        //playerCamera.GetComponent<MainCamera>().PlayerHide((angleX <= -hideAngle));
    }

    /// <summary> 操作入力の取得 </summary>
    void GetInput()
    {
        if (!UnlockManager.limit[UnlockState.move]) return;
        Hor = Input.GetAxis("Hor");
        Ver = Input.GetAxis("Ver");
        angle = new Vector3(Input.GetAxis("CameraHorizontal"), Input.GetAxis("CameraVertical"));
    }

    /// <summary> 装備中の武器の取得 </summary>
    public bool GetWeapon()
    {
        return isWeaponBeacon;
    }

    /// <summary> リスポーン </summary>
    public void Respawn()
    {
        //if (Annihilation()) return;
        //if (IsDead)
        //{
            //移動を殺す
            rb.velocity = Vector3.zero;
        residue--; //死亡可能回数の減少

        if (Annihilation()) return;
        GameTextController.TextStart(3);

        //if (Input.GetButtonDown("Select"))
        //{
        //初期位置へ
        transform.position = startPosition;
        transform.rotation = startRotation;
        playerCamera.transform.rotation = startCameraRotation;
        //ビッグエネミーを向く
        //Vector3 targetPos = BigEnemyScripts.mTransform.position;
        //targetPos.y = transform.position.y;
        //        transform.LookAt(targetPos);
                IsDead = false;
           // }
        //}
    }

    /// <summary> 残機０で死亡時 </summary>
    public bool Annihilation()
    {
        return (residue <= 0) ? true : false;
    }

    public int GetResidue()
    {
        return residue;
    }

    /// <summary> 死亡処理 </summary>
    void Death()
    {
        //カメラは破壊しない
        Camera.main.transform.parent = null;
        //Destroy(GameObject.FindGameObjectWithTag("Player"));
        gameObject.SetActive(false);
    }
}