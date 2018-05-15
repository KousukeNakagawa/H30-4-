using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject player;
    Transform root;
    List<Renderer> renderers = new List<Renderer>();

    //移動スピード
    [SerializeField] [Range(0.1f, 10)] float recoverySpeed = 3f;

    Vector3 startPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

    void Update()
    {
        AutoCameraControl();
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
}