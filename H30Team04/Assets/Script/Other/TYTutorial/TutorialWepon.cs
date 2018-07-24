using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWepon : MonoBehaviour {

    [SerializeField] GameObject beaconGun;
    [SerializeField] GameObject snipeGun;
    [SerializeField] GameObject beaconBullet;
    [SerializeField] GameObject snipeBullet;
    [SerializeField] GameObject rippel;
    [SerializeField] GameObject laser;
    LineRenderer line;

    //レーザーポインターの幅
    [SerializeField, Range(0.01f, 0.1f)] float laserWide = 0.1f;

    //着弾点エフェクトを浮かす値
    [SerializeField, Range(0, 3)] float effectPos = 0.1f;
    //スナイパーライフルのクールタイム
    [SerializeField, Range(0, 3)] float snipeCoolTime = 1;
    static float backupCoolTime;
    static float reloadTime;

    bool isSnipeFire = true;
    bool isreload = false;
    bool isLaserHit = false;
    float laserLength;

    public TutorialManager_T tmane;

    public Material blinemate;
    public Material rlinemate;

    private GameObject shotbeacon;

    /// <summary> 装備中の武器（true = beacon / false = snipe） </summary>
    public static bool WeaponBeacon { get; private set; }

    /// <summary> 武器を構えているか </summary>
    public static bool IsSetup { get; private set; }

    /// <summary> 地面に向かっているか </summary>
    public static bool IsFloorHit { get; private set; }

    AudioSource audioSourse;
    [SerializeField]
    AudioClip[] SE = new AudioClip[4];

    /// <summary> 使用するSEを分かりやすくするため </summary>
    enum SEs
    {
        fireBeacon, fireSnipe, reload, change
    }
    SEs audios = SEs.fireBeacon;
    Dictionary<SEs, AudioClip> playerSE = new Dictionary<SEs, AudioClip>();

    /// <summary> ビーコンの角度 </summary>
    public static Quaternion BeaconFieldAngle { get; private set; }
    /// <summary> ビーコンの角度 </summary>
    public static Quaternion BeaconBuildAngle { get; private set; }

    /// <summary> ビーコンの着弾点 </summary>
    public static Vector3 BeaconHitPos { get; private set; }

    /// <summary> 波紋エフェクトとの距離 </summary>
    public static float RippelDis { get; private set; }


    void Start()
    {
        //初期装備はビーコン
        WeaponBeacon = true;
        IsSetup = false;
        line = laser.GetComponent<LineRenderer>();

        audioSourse = gameObject.GetComponent<AudioSource>();
        //audioSourse.volume = 0.2f;
        //SEの読込
        for (int i = 0; i < SE.Length; i++)
            playerSE.Add(audios + i, SE[0 + i]);

        backupCoolTime = snipeCoolTime;
        reloadTime = snipeCoolTime / 2;
        WeaponSet();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        WeaponSet();
        laser.SetActive(tmane.GetState() >= TutorialState_T.BEACON && tmane.IsReaded());
        rippel.SetActive(tmane.GetState() >= TutorialState_T.BEACON&& tmane.IsReaded());
        if (!tmane.IsReaded() || tmane.GetState() < TutorialState_T.CAMERA) return;

        //回転入力
        if (Input.GetAxis("CameraHorizontal") != 0 || Input.GetAxis("CameraVertical") != 0)
            IsSetup = true;

        //if (!laser.activeSelf) laser.SetActive(true);

        ChangeWeapon();
    }

    void LateUpdate()
    {
        Shooting();
        //laser.SetActive(tmane.GetState() < TutorialState_T.BEACON);
    }

    /// <summary> 武器の変更処理 </summary>
    void ChangeWeapon()
    {
        if (!tmane.IsReaded() || tmane.GetState() < TutorialState_T.SNIPER) return;
        if (Input.GetButtonDown("newWeaponChange"))
        {
            //武器変更SE
            audioSourse.PlayOneShot(playerSE[SEs.change]);
            //武器の変更
            WeaponBeacon = !WeaponBeacon;
            IsSetup = true;
        }
    }

    /// <summary> レーザーポインターの描画 </summary>
    void Shooting()
    {
        if (!tmane.IsReaded() || tmane.GetState() < TutorialState_T.BEACON || Camera.main == null) return;

        var rayMuzzle = (WeaponBeacon) ?
            beaconGun.transform.Find("BeaconMuzzle") : snipeGun.transform.Find("SnipeMuzzle");

        //発射位置・方向
        var ray = new Ray(rayMuzzle.position, Camera.main.transform.forward);

        //武器によって長さが変わる
        var rayLength = (WeaponBeacon) ?
            BeaconBullet.GetRangeDistance() : SniperBullet.GetRangeDistance();

        //武器によって色が変わる
        var rayColor = (WeaponBeacon) ? blinemate : rlinemate;

        //クールタイム
        var isFire = isSnipeFire;

        //クールタイム処置
        if (!isSnipeFire)
        {
            snipeCoolTime -= Time.deltaTime;
            if (snipeCoolTime <= reloadTime && !isreload)
            {
                audioSourse.PlayOneShot(playerSE[SEs.reload], 0.5f);
                isreload = true;
            }
            if (snipeCoolTime <= 0)
            {
                isSnipeFire = true;
                isreload = false;
                snipeCoolTime = backupCoolTime;
            }
        }

        var fireMuzzle = (WeaponBeacon) ?
            rayMuzzle : snipeGun.transform.Find("FireMuzzle");

        //射撃可能なら射撃
        if (Input.GetAxis("newFire") < 0 && isFire && (IsSetup || TutorialPlayer.IsMove)) Fire(ray, fireMuzzle);

        //レイの衝突判定用
        RaycastHit hit;

        //レーザーの長さ
        if (!isLaserHit) laserLength = rayLength;

        if (IsSetup || TutorialPlayer.IsMove)
            LaserPointer(ray.origin, ray.direction * laserLength, rayColor, laserWide);
        else
            LaserPointer(ray.origin, Vector3.zero, rayColor, laserWide);
        
        // Rayがビルや地面・敵・射影機に衝突したら
        if (Physics.Raycast(ray, out hit, rayLength, LayerMask.GetMask("Building", "Enemy", "Xray")))
        {
            // 着弾点エフェクトの表示
            rippel.SetActive(true);
            // 着弾点エフェクトの角度と位置の指定
            TutorialAim.AimMotion(hit.normal, hit.point);
            // 着弾点エフェクトが近いと着弾点エフェクトを小さくする処理
            RippelDis = Vector3.Distance(transform.position, rippel.transform.position);

            // ビーコンの着弾時の角度の指定
            BeaconFieldAngle = Quaternion.LookRotation(hit.normal + new Vector3(90, 0));
            BeaconBuildAngle = Quaternion.LookRotation(hit.normal + new Vector3(0, -90, 0));

            // 発射した瞬間の位置の取得
            if (Input.GetAxis("newFire") < 0) BeaconHitPos = hit.point;

            isLaserHit = true;
            if (isLaserHit) laserLength = Vector3.Distance(hit.point, ray.origin);

            if (hit.collider.CompareTag("Field")) IsFloorHit = true;
            else IsFloorHit = false;
        }
        else
        {
            isLaserHit = false;
            rippel.SetActive(false);
        }
    }

    public static Quaternion BeaconAngle(bool isField = true)
    {
        return (isField) ? BeaconFieldAngle : BeaconBuildAngle;
    }

    /// <summary> 装備中の武器の使用 </summary>
    void Fire(Ray ray, Transform muzzle)
    {
        //他のビーコンを削除
        if (WeaponBeacon) OtherBeaconDedtroy();

        //発射する武器の指定
        var weapon = (WeaponBeacon) ? Instantiate(beaconBullet) : Instantiate(snipeBullet);

        //発射位置
        weapon.transform.position = muzzle.position - Vector3.up * 0.2f + Vector3.forward * 0.2f;

        //装備している武器で射撃
        if (WeaponBeacon)
        {
            //GameTextController.TextStart(1);
            weapon.GetComponent<TutorialBeacon>().Fire(ray.direction);
            audioSourse.PlayOneShot(playerSE[SEs.fireBeacon], 0.5f);
            isSnipeFire = false;
        }
        else
        {
            weapon.GetComponent<SniperBullet>().Fire(ray.direction);
            audioSourse.PlayOneShot(playerSE[SEs.fireSnipe], 0.5f);
            isSnipeFire = false;
        }
    }

    /// <summary> ビーコン重複防止 </summary>
    public void OtherBeaconDedtroy()
    {
        var beforBeacon = GameObject.FindGameObjectWithTag("Beacon");
        var beforBullet = GameObject.FindGameObjectWithTag("BeaconBullet");
        if (beforBeacon != null) Destroy(beforBeacon);
        if (beforBullet != null) Destroy(beforBullet);
    }

    /// <summary> 予測線の描画 </summary>
    void LaserPointer(Vector3 p1, Vector3 p2, Material mat, float width)
    {
        line.SetPosition(0, p1);
        line.SetPosition(1, p1 + p2);
        line.material = mat;
        line.startWidth = width;
        line.endWidth = width;
    }

    /// <summary> 装備中の武器の描画オンオフ </summary>
    void WeaponSet()
    {
        beaconGun.SetActive(WeaponBeacon);
        snipeGun.SetActive(!WeaponBeacon);
    }
}
