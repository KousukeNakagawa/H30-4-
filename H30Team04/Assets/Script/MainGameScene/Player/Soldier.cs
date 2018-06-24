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
    [SerializeField, Tooltip("D0515029:smdimport")] GameObject body;
    [SerializeField, Tooltip("ビーコンガン")] GameObject beacon;
    [SerializeField, Tooltip("スナイパーライフル")] GameObject snipe;
    List<GameObject> bodys = new List<GameObject>();
    [SerializeField, Range(0, 10)] float invincibleTime = 3;
    [SerializeField, Range(1, 10)] float fade = 2;
    float backupInvincibleTime;
    /// <summary> 無敵状態かどうか </summary>
    bool isInvincible = false;
    bool invincibleFade = false;
    ////プレイヤーを透過する角度
    //[SerializeField, Range(0, 90)] float hideAngle = 35;

    //カメラ反転のON/OFF
    [SerializeField] bool inverted = false;

    Rigidbody rb;

    //操作入力取得用
    float Hor;
    float Ver;
    Vector3 angle;

    //死亡可能回数
    [SerializeField] int residue = 3;

    [SerializeField] bool isUnlock = true;

    AudioSource audioSourse;
    //[SerializeField] AudioClip SE;

    //リスポーン用
    Vector3 startPosition;
    Quaternion startRotation;
    Quaternion startCameraRotation;

    List<Renderer> renderers = new List<Renderer>();
    float invincibleAlpha = 1;

    public static Transform Transform_ { get; private set; }

    public static bool IsDead { get; private set; }
    public static bool IsStop { get; set; }

    /// <summary> 移動しているかどうか（アニメーション用） </summary>
    public static bool IsMove { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSourse = gameObject.AddComponent<AudioSource>();
        audioSourse.spatialBlend = 0.8f;
        IsDead = false;
        IsStop = false;
        Transform_ = transform;

        startPosition = transform.position;
        startRotation = transform.rotation;
        startCameraRotation = playerCamera.transform.rotation;

        UnlockManager.AllSet(isUnlock);
        backupInvincibleTime = invincibleTime;

        foreach (var sniper in body.GetComponentsInChildren<SkinnedMeshRenderer>()) renderers.Add(sniper);
        //ライフルのメッシュの取得
        foreach (var rifle in beacon.GetComponentsInChildren<MeshRenderer>()) renderers.Add(rifle);
        foreach (var rifle in snipe.GetComponentsInChildren<MeshRenderer>()) renderers.Add(rifle);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (!SEManager.IsEndSE) return;

        // LT押している間は無敵
        isInvincible = Xray_SSS.IsShutterChance;

        GetInput();
        Rotation();
        Invincible();

        if (Annihilation()) Death(); //３回やられたら死亡

        //START:初期位置へワープ（デバック用）
        if (Input.GetButtonDown("Restart")) Damage();

        //Respawn();
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0) return;
        if (IsStop) rb.velocity = Vector3.zero;

        Move();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("BigEnemy")) Damage();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile")) Damage();
    }

    /// <summary> 移動処理 </summary>
    void Move()
    {
        if (!UnlockManager.Limiter[UnlockState.move]) return;

        var speed = (IsStop) ? 0 : this.speed;

        var move = ((transform.forward * Ver) + (transform.right * Hor)).normalized;
        rb.velocity = move * speed * Time.deltaTime;

        IsMove = (rb.velocity != Vector3.zero);
    }

    /// <summary> 回転処理 </summary>
    void Rotation()
    {
        if (!UnlockManager.Limiter[UnlockState.move]) return;
        //カメラ反転の対応
        float changer = (inverted) ? 1 : -1;
        //AIM時の対応
        float rotateSpeed = (Input.GetButton("Shooting")) ? aimRotateSpeed : _rotateSpeed;

        //LT押していない間
        if (!MainCamera.IsMove)
        {
            //プレイヤーの回転
            transform.eulerAngles += new Vector3(0, angle.x) * rotateSpeed * Time.deltaTime;

            //カメラとライフルの回転
            playerCamera.transform.eulerAngles += new Vector3(angle.y * changer, angle.x) * rotateSpeed * Time.deltaTime;

            //-180＜上下の動き＜180に変更
            float angleX = (180 <= playerCamera.transform.eulerAngles.x) ?
                playerCamera.transform.eulerAngles.x - 360 : playerCamera.transform.eulerAngles.x;

            //上下の制限
            playerCamera.transform.eulerAngles =
                new Vector3(Mathf.Clamp(angleX, -maxAngle, -minAngle), playerCamera.transform.eulerAngles.y, playerCamera.transform.eulerAngles.z);
        }
        //プレイヤーの透過
        //playerCamera.GetComponent<MainCamera>().PlayerHide((angleX <= -hideAngle));
    }

    /// <summary> 操作入力の取得 </summary>
    void GetInput()
    {
        if (!UnlockManager.Limiter[UnlockState.move]) return;
        Hor = Input.GetAxis("Hor");
        Ver = Input.GetAxis("Ver");
        angle = new Vector3(Input.GetAxis("CameraHorizontal"), Input.GetAxis("CameraVertical"));
    }

    /// <summary> ダメージ処理 </summary>
    public void Damage()
    {
        // 無敵状態ならスルー
        if (isInvincible) return;

        // 移動を殺す
        rb.velocity = Vector3.zero;
        // 残機を減らす
        residue--;

        //残機がなくなったら終わり
        if (Annihilation()) return;
        // 死亡処理
        IsDead = false;

        // 無敵状態になる
        isInvincible = true;
    }

    /// <summary> 無敵処理 </summary>
    void Invincible()
    {
        //無敵状態じゃなかったら無視
        if (!isInvincible) return;

        // カウントダウン
        invincibleTime -= Time.deltaTime;

        // 無敵時間終了時処理
        if (invincibleTime <= 0)
        {
            invincibleAlpha = 1;
            invincibleTime = backupInvincibleTime;
            // 無敵状態解除
            isInvincible = false;
        }

        // 透明度
        if (invincibleAlpha <= 0) invincibleFade = true;
        if (invincibleAlpha >= 1) invincibleFade = false;
        invincibleAlpha = (invincibleFade) ?
            invincibleAlpha += fade * Time.deltaTime : invincibleAlpha -= fade * Time.deltaTime;

        // プレイヤーの点滅
        foreach (var rend in renderers)
            foreach (var material in rend.materials)
                material.color = new Color(1, 1, 1, invincibleAlpha);
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