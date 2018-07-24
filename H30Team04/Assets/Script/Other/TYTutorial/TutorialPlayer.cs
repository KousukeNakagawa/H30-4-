using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour {

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
    ////プレイヤーを透過する角度
    //[SerializeField, Range(0, 90)] float hideAngle = 35;

    //カメラ反転のON/OFF
    [SerializeField] bool inverted = false;

    Rigidbody rb;

    //操作入力取得用
    float Hor;
    float Ver;
    Vector3 angle;
    
    AudioSource audioSourse;

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
    public static bool IsRotate { get; private set; }

    public TutorialManager_T tmane;

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

        foreach (var sniper in body.GetComponentsInChildren<SkinnedMeshRenderer>()) renderers.Add(sniper);
        //ライフルのメッシュの取得
        foreach (var rifle in beacon.GetComponentsInChildren<MeshRenderer>()) renderers.Add(rifle);
        foreach (var rifle in snipe.GetComponentsInChildren<MeshRenderer>()) renderers.Add(rifle);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (!tmane.IsReaded()) return;

        GetInput();
        Rotation();
        
        
    }

    void FixedUpdate()
    {
        //if (!tmane.IsReaded())
        //{
        //    rb.velocity = Vector3.zero;
        //    return;
        //}
        //if (IsStop || !tmane.IsReaded()) rb.velocity = Vector3.zero;

        Move();
        if(transform.position.x > 85|| transform.position.x < -85 ||
            transform.position.z > 85 || transform.position.z < -85)
        {
            if (tmane.IsReaded()) tmane.ResetState(ResetConditions.PLAYERAWAY);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("BigEnemy"))
        {
            if (!tmane.IsReaded() || tmane.GetState() != TutorialState_T.BEACON && tmane.GetState() != TutorialState_T.SNIPER) return;

            tmane.ResetState(ResetConditions.PLAYERDEAD);
            GetComponent<Collider>().enabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    /// <summary> 移動処理 </summary>
    void Move()
    {
        //if (!UnlockManager.Limiter[UnlockState.move]) return;

        var speed = (IsStop || !tmane.IsReaded() ||
            tmane.GetState() != TutorialState_T.MOVE && tmane.GetState() != TutorialState_T.BEACON && tmane.GetState() != TutorialState_T.SNIPER) ? 0 : this.speed;

        var move = ((transform.forward * Ver) + (transform.right * Hor)).normalized;
        rb.velocity = move * speed * Time.deltaTime;

        IsMove = (rb.velocity != Vector3.zero);
    }

    /// <summary> 回転処理 </summary>
    void Rotation()
    {
        if (!tmane.IsReaded() || TutorialState_T.CAMERA > tmane.GetState()) return;
        //カメラ反転の対応
        float changer = (inverted) ? 1 : -1;
        //AIM時の対応
        float rotateSpeed = (Input.GetButton("Shooting")) ? aimRotateSpeed : _rotateSpeed;

        if (Sensitivity.getSensitivitytest() != 0) rotateSpeed *= Sensitivity.getSensitivitytest() / 5;

        //LT押していない間
        if (!TutorialMainCamera.IsMove)
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
        Hor = Input.GetAxis("Hor");
        Ver = Input.GetAxis("Ver");
        angle = new Vector3(Input.GetAxis("CameraHorizontal"), Input.GetAxis("CameraVertical"));

        IsRotate = (angle != Vector3.zero);
    }

    /// <summary> リスポーン </summary>
    public void Respawn()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        //移動を殺す
        rb.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = true;


        //初期位置へ
        transform.position = startPosition;
        transform.rotation = startRotation;
        playerCamera.transform.rotation = startCameraRotation;
        IsDead = false;

        GetComponent<TutorialWepon>().OtherBeaconDedtroy();
        TutorialXray_SSS.IsShutterChance = false;
    }
    
}
