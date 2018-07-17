using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    /// <summary> 使用するSEの列挙型 </summary>
    enum WeaponSE
    {
        /// <summary> ビーコン射出音 </summary>
        fireBeacon,
        /// <summary> スナイパー射出音 </summary>
        fireSnipe,
        /// <summary> リロード音 </summary>
        reload,
        /// <summary> 武器変更音 </summary>
        change
    }

    /*** 必要ゲームオブジェクト ***/
    /// <summary> ビーコン銃 </summary>
    [SerializeField] GameObject beaconGun;
    /// <summary> スナイパーライフル </summary>
    [SerializeField] GameObject snipeGun;
    /// <summary> ビーコン弾 </summary>
    [SerializeField] GameObject beaconBullet;
    /// <summary> スナイパー弾 </summary>
    [SerializeField] GameObject snipeBullet;
    /// <summary> 着弾点エフェクト </summary>
    [SerializeField] GameObject rippel;
    /// <summary> レーザーポインター </summary>
    [SerializeField] GameObject laser;

    /// <summary> ラインレンダラー </summary>
    LineRenderer line;

    /*** 情報 ***/
    /// <summary> レーザーポインターの幅 </summary>
    [SerializeField, Range(0.01f, 0.1f)] float laserWide = 0.1f;
    /// <summary> 着弾点エフェクトを浮かす値 </summary>
    [SerializeField, Range(0, 3)] float effectPos = 0.1f;
    /// <summary> スナイパーライフルのクールタイム </summary>
    [SerializeField, Range(0, 3)] float snipeCoolTime = 1;

    /// <summary> レーザーポインターが対象に当たっているか </summary>
    bool isLaserHit = false;
    /// <summary> レーザーポインターの長さ </summary>
    float laserLength;

    /// <summary> スナイパーライフルのクールタイムの初期値 </summary>
    static float backupCoolTime;
    /// <summary> リロード時間 </summary>
    static float reloadTime;
    /// <summary> スナイパーライフルを使用できるかのフラグ </summary>
    bool isSnipeFire = true;
    /// <summary> リロードのフラグ </summary>
    bool isReload = false;

    /*** 音情報 ***/
    /// <summary> 音情報 </summary>
    AudioSource audioSourse;
    /// <summary> SEを管理 </summary>
    [SerializeField] AudioClip[] SEs = new AudioClip[4];
    /// <summary> 使用するSE </summary>
    WeaponSE SE = WeaponSE.fireBeacon;
    /// <summary> 使用するSEをステートパターンで管理 </summary>
    Dictionary<WeaponSE, AudioClip>
        playerSE = new Dictionary<WeaponSE, AudioClip>();

    /*** プロパティ ***/
    /// <summary> 装備中の武器（true = beacon / false = snipe） </summary>
    public static bool IsWeaponBeacon { get; private set; }

    /// <summary> 武器を構えているか </summary>
    public static bool IsSetup { get; private set; }

    /// <summary> 地面に向かっているか </summary>
    public static bool IsFloorHit { get; private set; }

    /// <summary> 地面に付着時のビーコンの角度 </summary>
    public static Quaternion BeaconFieldAngle { get; private set; }

    /// <summary> ビルに付着時のビーコンの角度 </summary>
    public static Quaternion BeaconBuildAngle { get; private set; }

    /// <summary> ビーコンの着弾点 </summary>
    public static Vector3 BeaconHitPos { get; private set; }

    /// <summary> 波紋エフェクトとの距離 </summary>
    public static float RippelDis { get; private set; }

    void Start()
    {
        // ビーコンを初期装備にする
        IsWeaponBeacon = true;
        IsSetup = false;
        backupCoolTime = snipeCoolTime;
        reloadTime = snipeCoolTime / 2;

        line = laser.GetComponent<LineRenderer>();

        audioSourse = gameObject.AddComponent<AudioSource>();
        //audioSourse.volume = 0.2f;
        // SEの取得
        for (int i = 0; i < SEs.Length; i++)
            playerSE.Add(SE + i, SEs[0 + i]);
    }

    void Update()
    {
        // 装備中の武器の表示
        WeaponSetActive();

        // 一時停止中は何もしない
        if (Time.timeScale == 0) return;
        // [move]がアンロックされていないなら何もしない
        if (!UnlockManager.Limiter[UnlockState.move]) return;

        // 回転している間は武器を構える
        if (Input.GetAxis("CameraHorizontal") != 0 ||
            Input.GetAxis("CameraVertical") != 0) IsSetup = true;

        // 武器の変更処理
        ChangeWeapon();
    }

    void LateUpdate()
    {
        // 一時停止中は何もしない
        if (Time.timeScale == 0) return;

        // レーザーポインターの描画
        Shooting();
        // レーザーポインターの描画のオンオフ
        laser.SetActive(UnlockManager.Limiter[UnlockState.laserPointer]);
    }

    /// <summary> 武器の変更処理 </summary>
    void ChangeWeapon()
    {
        // [snipe]がアンロックされていないなら何もしない
        if (!UnlockManager.Limiter[UnlockState.snipe]) return;

        // RBを押したら 武器を変更する
        if (Input.GetButtonDown("newWeaponChange"))
        {
            IsSetup = true;
            //武器の変更
            IsWeaponBeacon = !IsWeaponBeacon;
            //武器変更SE
            audioSourse.PlayOneShot(playerSE[WeaponSE.change]);
        }
    }

    /// <summary> レーザーポインターの描画 </summary>
    void Shooting()
    {
        // [beacon]がアンロックされていないなら何もしない カメラがないときも何もしない
        if (!UnlockManager.Limiter[UnlockState.beacon] || Camera.main == null) return;

        //レイの衝突判定用
        RaycastHit hit;
        // レーザーポインターの射出位置
        var rayMuzzle = (IsWeaponBeacon) ?
            beaconGun.transform.Find("BeaconMuzzle") : snipeGun.transform.Find("SnipeMuzzle");
        // レーザーポインターのRay
        var laserRay = new Ray(rayMuzzle.position, Camera.main.transform.forward);
        // 武器ごとのレーザーポインターの長さ（射程）
        var rayLength = (IsWeaponBeacon) ?
            BeaconBullet.RangeDistance_ : SniperBullet.RangeDistance_;
        // 描画させるレーザーポインターの長さ
        var length = (IsSetup || Soldier.IsMove) ? laserRay.direction * laserLength : Vector3.zero;
        // レーザーポインターの色
        var rayColor = (IsWeaponBeacon) ? Color.blue : Color.red;

        // 武器を使用できるかのフラグ
        var isFire = isSnipeFire;
        // 弾の射出位置
        var fireMuzzle = (IsWeaponBeacon) ?
            rayMuzzle : snipeGun.transform.Find("FireMuzzle");

        // スナイパーライフルのクールタイム
        if (!isSnipeFire)
        {
            // クールタイムのカウントダウン
            snipeCoolTime -= Time.deltaTime;
            // リロード音の発生条件
            if (snipeCoolTime <= reloadTime && !isReload)
            {
                // リロード音を鳴らす
                audioSourse.PlayOneShot(playerSE[WeaponSE.reload], 0.5f);
                isReload = true;
            }
            if (snipeCoolTime <= 0)
            {
                // クールタイムの回復
                isSnipeFire = true;
                isReload = false;
                snipeCoolTime = backupCoolTime;
            }
        }

        // 射撃可能状態で 武器を構えている状態なら RTボタンを押すと射撃
        if (Input.GetAxis("newFire") < 0 &&
            isFire && (IsSetup || Soldier.IsMove)) Fire(laserRay, fireMuzzle);

        //レーザーの長さ
        if (!isLaserHit) laserLength = rayLength;
        // レーザーポインターの描画
        LaserPointer(laserRay.origin, length, rayColor, laserWide);

        // Rayがビルや地面・敵・射影機に衝突したら
        if (Physics.Raycast(laserRay, out hit, rayLength, LayerMask.GetMask("Building", "Enemy", "Xray")))
        {
            // 着弾点エフェクトの表示
            rippel.SetActive(true);
            // 着弾点エフェクトの角度と位置の指定
            rippel.transform.rotation = Quaternion.LookRotation(hit.normal);
            rippel.transform.position = hit.point + hit.normal * effectPos;
            // 着弾点エフェクトが近いと着弾点エフェクトを小さくする処理
            RippelDis = Vector3.Distance(transform.position, rippel.transform.position);

            // ビーコンの着弾時の角度の指定
            BeaconFieldAngle = Quaternion.LookRotation(hit.normal + new Vector3(90, 0));
            BeaconBuildAngle = Quaternion.LookRotation(hit.normal + new Vector3(0, -90, 0));

            // 発射した瞬間の位置の取得
            if (Input.GetAxis("newFire") < 0) BeaconHitPos = hit.point;

            // レーザーポインターの衝突フラグを立たせる
            isLaserHit = true;
            // レーザーポインターの長さを衝突した位置までにする
            if (isLaserHit) laserLength = Vector3.Distance(hit.point, laserRay.origin);

            // 地面に衝突したか
            if (hit.collider.CompareTag("Field")) IsFloorHit = true;
            else IsFloorHit = false;
        }
        else
        {
            // レーザーポインターの衝突フラグを折る
            isLaserHit = false;
            // 着弾点エフェクトの表示を消す
            rippel.SetActive(false);
            // ビーコンの着弾位置を虚空に（レーザーポインターの終点）
            BeaconHitPos = laserRay.direction * laserLength;
        }
    }

    /// <summary> ビーコンの着弾角度 </summary>
    public static Quaternion BeaconAngle(bool isField = true)
    {
        return (isField) ? BeaconFieldAngle : BeaconBuildAngle;
    }

    /// <summary> 装備中の武器の使用 </summary>
    void Fire(Ray ray, Transform muzzle)
    {
        // 他のビーコンを削除する（ビーコンは同時に２つ以上存在しない）
        if (IsWeaponBeacon) OtherBeaconDedtroy();

        // 射出する弾
        var weapon = (IsWeaponBeacon) ?
            Instantiate(beaconBullet) : Instantiate(snipeBullet);

        // 射出位置
        weapon.transform.position = muzzle.position;

        /*** 装備している武器を使用 ***/
        // ビーコン
        if (IsWeaponBeacon)
        {
            //GameTextController.TextStart(1);
            weapon.GetComponent<BeaconBullet>().Fire(ray.direction);
            audioSourse.PlayOneShot(playerSE[WeaponSE.fireBeacon], 0.5f);
            isSnipeFire = false;
        }
        // スナイパーライフル
        else
        {
            weapon.GetComponent<SniperBullet>().Fire(ray.direction);
            audioSourse.PlayOneShot(playerSE[WeaponSE.fireSnipe], 0.5f);
            isSnipeFire = false;
        }
    }

    /// <summary> ビーコン重複防止 </summary>
    void OtherBeaconDedtroy()
    {
        // ビーコン状態のビーコン弾
        var beforBeacon = GameObject.FindGameObjectWithTag("Beacon");
        // 射出中のビーコン弾
        var beforBullet = GameObject.FindGameObjectWithTag("BeaconBullet");
        // 存在しているなら消す
        if (beforBeacon != null) Destroy(beforBeacon);
        if (beforBullet != null) Destroy(beforBullet);
    }

    /// <summary> 予測線の描画 </summary>
    void LaserPointer(Vector3 p1, Vector3 p2, Color c1, float width)
    {
        // 始点
        line.SetPosition(0, p1);
        // 終点
        line.SetPosition(1, p1 + p2);
        // 開始色
        line.startColor = c1;
        // 末端色
        line.endColor = c1;
        // 開始幅
        line.startWidth = width;
        // 末端幅
        line.endWidth = width;
    }

    /// <summary> 装備中の武器の描画オンオフ </summary>
    void WeaponSetActive()
    {
        beaconGun.SetActive(IsWeaponBeacon);
        snipeGun.SetActive(!IsWeaponBeacon);
    }
}