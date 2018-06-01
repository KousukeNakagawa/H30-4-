using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject player;
    PlayerBase playerBase;
    Transform root;
    List<Renderer> renderers = new List<Renderer>();

    //移動スピード
    [SerializeField] [Range(0.1f, 10)] float recoverySpeed = 3f;

    Vector3 startPos;

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerBase = player.GetComponent<PlayerBase>();
        //Projectionの情報
        root = transform.root;

        //初期位置のバックアップ
        startPos = transform.localPosition;

        //車のメッシュの取得
        foreach (var car in player.GetComponentsInChildren<SkinnedMeshRenderer>()) renderers.Add(car);
        //スナイパーのメッシュの取得
        foreach (var sniper in root.GetComponentsInChildren<SkinnedMeshRenderer>()) renderers.Add(sniper);
        //ライフルのメッシュの取得
        foreach (var rifle in root.GetComponentsInChildren<MeshRenderer>()) renderers.Add(rifle);
    }

    void LateUpdate()
    {
        RotateCameraAngle();
        //AutoCameraControl();
    }

    /// <summary>
    /// ＊壁抜け・床抜け防止
    /// </summary>
    void AutoCameraControl()
    {
        if (player == null) return;

        RaycastHit hit;

        //プレイヤーの位置からカメラにレイを飛ばし、ビルと床に衝突したら
        if (Physics.Linecast(root.position + Vector3.up, transform.position, out hit, LayerMask.GetMask("Building")))
            //レイの当たった場所がカメラの位置へ
            transform.position =
                Vector3.Lerp(transform.position, hit.point, recoverySpeed * Time.deltaTime);

        //当たっていなければ本来の位置へ戻る
        else transform.localPosition =
               Vector3.Lerp(transform.localPosition, startPos, recoverySpeed * Time.deltaTime);
    }

    /// <summary>
    /// プレイヤーの透過のオンオフ
    /// </summary>
    public void PlayerHide(bool hide)
    {
        var alpha = (hide) ? 0 : 3000;

        foreach (var items in renderers)
        {
            foreach (var item in items.materials)
            {
                item.SetFloat("_Mode", 2);
                item.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                item.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                item.SetInt("_ZWrite", 0);
                item.DisableKeyword("_ALPHATEST_ON");
                item.EnableKeyword("_ALPHABLEND_ON");
                item.DisableKeyword("_ALPHAPREMULTIPLY_ON");

                item.SetOverrideTag("RenderType", "Transparent");
                item.renderQueue = alpha;
            }
        }
    }

    /// <summary>
    /// ＊手動カメラアングル（右スティック）
    /// </summary>
    void RotateCameraAngle()
    {
        transform.position = player.transform.position - player.transform.forward*2 + Vector3.up * 1.5f;

        //カメラ反転の対応
        float changer = (inverted) ? 1 : -1;
        //AIM時の対応
        float rotateSpeed = (Input.GetButton("Shooting")) ? aimRotateSpeed : _rotateSpeed;

        //右パット入力の取得
        Vector3 angle = new Vector3(Input.GetAxis("CameraHorizontal"), Input.GetAxis("CameraVertical")) * Time.deltaTime;

        //カメラとライフルの回転
        transform.eulerAngles += new Vector3(angle.y * changer, angle.x) * rotateSpeed;

        //-180＜上下の動き＜180に変更
        float angleX = (180 <= transform.eulerAngles.x) ?
            transform.eulerAngles.x - 360 : transform.eulerAngles.x;

        //上下の制限
        transform.eulerAngles =
            new Vector3(Mathf.Clamp(angleX, -maxAngle, -minAngle), transform.eulerAngles.y, transform.eulerAngles.z);

        //プレイヤーの透過
        //playerCamera.GetComponent<MainCamera>().PlayerHide((angleX <= -hideAngle));

    }
}