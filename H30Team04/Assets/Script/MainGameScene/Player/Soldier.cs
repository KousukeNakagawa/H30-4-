using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    /*** 必要なゲームオブジェクト ***/
    /// <summary> プレイヤーカメラ </summary>
    [SerializeField] GameObject playerCamera;
    /// <summary> プレイヤーのモデル </summary>
    [SerializeField, Tooltip("D0515029:smdimport")] GameObject body;
    /// <summary> ビーコン銃 </summary>
    [SerializeField, Tooltip("ビーコンガン")] GameObject beacon;
    /// <summary> スナイパーライフル </summary>
    [SerializeField, Tooltip("スナイパーライフル")] GameObject snipe;

    /// <summary> 無敵時に透明になるモデルを管理 </summary>
    List<Renderer> renderers = new List<Renderer>();
    /// <summary> リジッドボディ </summary>
    Rigidbody rb;

    /*** 移動・回転 ***/
    /// <summary> 移動速度 </summary>
    [SerializeField, Range(10, 500)] float speed_ = 100;
    /// <summary> カメラ回転速度 </summary>
    [SerializeField, Range(1, 500)] int _rotateSpeed = 100;
    /// <summary> LB押しているときのカメラ回転速度 </summary>
    [SerializeField, Range(1, 100)] int aimRotateSpeed = 20;
    /// <summary> 下を向く時のカメラ移動速度 </summary>
    [SerializeField, Range(1, 10)] float downLookSpeed = 8;
    /// <summary> 最大上下角度 </summary>
    [SerializeField, Range(0, 80)] float maxAngle = 80;
    /// <summary> 最小上下角度 </summary>
    [SerializeField, Range(0, -80)] float minAngle = -25;
    /// <summary> カメラ反転のオンオフ </summary>
    [SerializeField] bool inverted = false;

    /// <summary> 左右の移動取得用 </summary>
    float Hor;
    /// <summary> 前後の移動取得用 </summary>
    float Ver;
    /// <summary> カメラの回転取得用 </summary>
    Vector3 angle;

    /*** 無敵時関連 ***/
    /// <summary> 無敵時間 </summary>
    [SerializeField, Range(0, 10)] float invincibleTime = 3;
    /// <summary> 無敵時のフェード速度 </summary>
    [SerializeField, Range(1, 10)] float fadeSpeed = 2;
    /// <summary> 最大無敵時間 </summary>
    float maxInvincibleTime;
    /// <summary> 無敵状態時の透明度 </summary>
    float invincibleAlpha = 1;
    /// <summary> ダメージを受けているかのフラグ </summary>
    bool isDamage = false;
    /// <summary> 無敵状態かどうか </summary>
    bool isInvincible = false;
    /// <summary> 無敵時のフェードの切替フラグ </summary>
    bool invincibleFade = false;

    /*** その他情報 ***/
    /// <summary> 残機 </summary>
    [SerializeField] int residue = 3;
    /// <summary> 行動制限 </summary>
    [SerializeField] bool isUnlock = true;

    /// <summary> プレイヤーが止まっているか </summary>
    public static bool isStop;

    /*** プロパティ ***/
    /// <summary> プレイヤーのトランスフォーム情報 </summary>
    public static Transform Transform_ { get; private set; }

    /// <summary> 移動しているかどうか（アニメーション用） </summary>
    public static bool IsMove { get; private set; }

    /// <summary> 下を見ているとき </summary>
    public static bool IsDownLook { get; private set; }

    [SerializeField]
    GlitchFx m_glitcFX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxInvincibleTime = invincibleTime;
        Transform_ = transform;
        isStop = false;

        // アンロック
        UnlockManager.AllSet(isUnlock);

        // 使用するモデルの格納
        foreach (var sniper in body.GetComponentsInChildren<SkinnedMeshRenderer>()) renderers.Add(sniper);
        foreach (var rifle in beacon.GetComponentsInChildren<MeshRenderer>()) renderers.Add(rifle);
        foreach (var rifle in snipe.GetComponentsInChildren<MeshRenderer>()) renderers.Add(rifle);

        //ノイズのActiveをfalseに
        m_glitcFX.enabled = false;
    }

    void Update()
    {
        // 一時致死中は何もしない
        if (Time.timeScale == 0) return;
        //if (!SEManager.IsEndSE) return;

        // LT押している間と被弾時は無敵状態になる
        isInvincible = (Xray_SSS.IsShutterChance || isDamage);

        // 無敵時の処理
        Invincible();

        // 残機が０になったら志望
        if (Annihilation()) Death();

        // [move]がアンロックされていないなら 何もしない
        if (!UnlockManager.Limiter[UnlockState.move]) return;
        // 入力の取得
        GetInput();
        // プレイヤーの回転
        Rotation();
    }

    void FixedUpdate()
    {
        // 一時停止中は何もしない
        if (Time.timeScale == 0) return;
        if (isStop) rb.velocity = Vector3.zero;

        // プレイヤーの移動
        Move();
    }

    void OnCollisionEnter(Collision other)
    {
        // ロボと衝突時の処理
        if (other.collider.CompareTag("BigEnemy")) Damage();
    }

    void OnTriggerEnter(Collider other)
    {
        // ミサイルと衝突時の処理
        if (other.CompareTag("Missile")) Damage();
    }

    /// <summary> 移動処理 </summary>
    void Move()
    {
        // [move]がアンロックされていないなら 移動できない
        if (!UnlockManager.Limiter[UnlockState.move]) return;

        // 移動速度（強制停止命令が下っているなら動かない）
        var speed = (isStop) ? 0 : speed_;
        // 操作入力を正規化した移動力
        var move = ((transform.forward * Ver) + (transform.right * Hor)).normalized;

        // 移動処理
        rb.velocity = move * speed * Time.deltaTime;

        // 移動しているなら 移動フラグをtrueにする
        IsMove = (rb.velocity != Vector3.zero);
    }

    /// <summary> 回転処理 </summary>
    void Rotation()
    {
        // カメラ反転の対応
        float changer = (inverted) ? 1 : -1;

        // カメラ回転速度（LB押している間はゆっくり）
        float rotateSpeed = (Input.GetButton("Shooting")) ? aimRotateSpeed : _rotateSpeed;

        // カメラがプレイヤーの後ろにいる間
        if (!MainCamera.IsNotPlayerBack)
        {
            // プレイヤーの回転
            transform.eulerAngles += new Vector3(0, angle.x) * rotateSpeed * Time.deltaTime;

            // カメラの回転
            playerCamera.transform.eulerAngles += new Vector3(angle.y * changer, angle.x) * rotateSpeed * Time.deltaTime;

            // X軸の回転（上下回転）を -180 ＜ 上下の動き ＜ 180 に制限
            float angleX = (180 <= playerCamera.transform.eulerAngles.x) ?
                playerCamera.transform.eulerAngles.x - 360 : playerCamera.transform.eulerAngles.x;

            // 上下回転の角度制限
            playerCamera.transform.eulerAngles =
                new Vector3(Mathf.Clamp(angleX, -maxAngle, -minAngle),
                playerCamera.transform.eulerAngles.y, playerCamera.transform.eulerAngles.z);

            // 基本 false
            IsDownLook = false;
            // カメラの上下回転が10～30度になると
            if (playerCamera.transform.eulerAngles.x > 10 && playerCamera.transform.eulerAngles.x <= 30)
            {
                // この条件時のみ true
                IsDownLook = true;
                // 位置
                var pos = transform.position + Vector3.up * 1.5f + transform.forward;
                // カメラを移動させる
                playerCamera.transform.position =
                    Vector3.Lerp(playerCamera.transform.position, pos, downLookSpeed * Time.deltaTime);
            }

            print(IsDownLook);
        }
    }

    /// <summary> 操作入力の取得 </summary>
    void GetInput()
    {
        // 前後への入力の取得
        Hor = Input.GetAxis("Hor");
        // 左右への入力の取得
        Ver = Input.GetAxis("Ver");
        // 上下左右回転の入力の取得
        angle = new Vector3(Input.GetAxis("CameraHorizontal"), Input.GetAxis("CameraVertical"));
    }

    /// <summary> 残機減少処理 </summary>
    public void Damage()
    {
        // 無敵状態なら残機が減らない
        if (isInvincible) return;

        // 残機を減らす
        residue--;
        // 博士のコメント
        GameTextController.TextStart(3);

        //残機がなくなったら 無敵になれない
        if (Annihilation()) return;
        // 無敵状態になる
        isDamage = true;
        // 強制的に無敵状態へ
        isInvincible = true;
    }

    /// <summary> 無敵状態処理 </summary>
    void Invincible()
    {
        // 被弾フラグが立っていないなら何もしない
        if (!isDamage) return;

        //ノイズのActiveをtrueに
        m_glitcFX.enabled = true;
        // 無敵時間のカウントダウン
        invincibleTime -= Time.deltaTime;
        // 無敵時間終了時処理
        if (invincibleTime <= 0)
        {
            //ノイズのActiveをfalseに
            m_glitcFX.enabled = false;
            // 透明解除
            invincibleAlpha = 1;
            // 無敵時間のリセット
            invincibleTime = maxInvincibleTime;
            // 無敵状態解除
            isDamage = false;
        }

        // 透明度の変化量の変更処理
        if (invincibleAlpha <= 0) invincibleFade = true;
        if (invincibleAlpha >= 1) invincibleFade = false;
        // 透明度を変化させる
        invincibleAlpha = (invincibleFade) ?
            invincibleAlpha += fadeSpeed * Time.deltaTime : invincibleAlpha -= fadeSpeed * Time.deltaTime;

        // 格納したモデルが点滅する
        foreach (var renderer in renderers)
            foreach (var material in renderer.materials)
                material.color = new Color(1, 1, 1, invincibleAlpha);
    }

    /// <summary> 残機がなくなったか </summary>
    public bool Annihilation()
    {
        return (residue <= 0) ? true : false;
    }

    /// <summary> 残機のゲッター </summary>
    public int GetResidue()
    {
        return residue;
    }

    /// <summary> 死亡処理 </summary>
    void Death()
    {
        // カメラの親子関係の解除
        Camera.main.transform.parent = null;
        // 自身の表示を消す
        gameObject.SetActive(false);
    }
}