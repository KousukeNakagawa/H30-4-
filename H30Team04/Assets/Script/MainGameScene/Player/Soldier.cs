using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject rifle;
    List<GameObject> cameras = new List<GameObject>();

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

        cameras.Add(playerCamera);
        cameras.Add(rifle);
    }

    void Update()
    {
        if (!SEManager.IsEndSE) return;

        GetInput();
        Rotation();

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

    /// <summary> 回転処理 </summary>
    void Rotation()
    {
        //カメラ反転の対応
        float changer = (inverted) ? 1 : -1;
        //AIM時の対応
        float rotateSpeed = (Input.GetButton("Shooting")) ? aimRotateSpeed : _rotateSpeed;

        //右パット入力の取得
        Vector3 angle = new Vector3(Input.GetAxis("CameraHorizontal"), Input.GetAxis("CameraVertical"));

        //プレイヤーの回転
        transform.eulerAngles += new Vector3(0, angle.x) * rotateSpeed * Time.deltaTime;

        //カメラとライフルの回転
        playerCamera.transform.eulerAngles += new Vector3(angle.y * changer, angle.x) * rotateSpeed * Time.deltaTime;
        //rifle.transform.eulerAngles += new Vector3(angle.y * changer, angle.x) * rotateSpeed * Time.deltaTime;
        rifle.transform.forward = playerCamera.transform.forward;

        Debug.Log(transform.eulerAngles.y + "///" + rifle.transform.eulerAngles.y);

        //-180＜上下の動き＜180に変更
        float angleX = (180 <= playerCamera.transform.eulerAngles.x) ?
            playerCamera.transform.eulerAngles.x - 360 : playerCamera.transform.eulerAngles.x;

        //上下の制限
        playerCamera.transform.eulerAngles =
            new Vector3(Mathf.Clamp(angleX, -maxAngle, -minAngle), playerCamera.transform.eulerAngles.y, playerCamera.transform.eulerAngles.z);

        //プレイヤーの透過
        //playerCamera.GetComponent<MainCamera>().PlayerHide((angleX <= -hideAngle));
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