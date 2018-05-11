using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject cameraContoller;

    List<Renderer[]> renderers = new List<Renderer[]>();

    //移動スピード
    [SerializeField] [Range(0.1f, 10)] float recoverySpeed = 3f;
    [SerializeField, Range(0, 0.5f)] float alpha = 0; //透過時の透明度

    Vector3 startPos;
    Transform root;

    bool isHide;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //CameraControllerの情報
        root = transform.root;
        ////車のメッシュ
        foreach (var item in player.GetComponentsInChildren<SkinnedMeshRenderer[]>())
        {
            renderers.Add(item);
        }
        //スナイパーのメッシュ
        foreach (var item in cameraContoller.GetComponentsInChildren<SkinnedMeshRenderer[]>())
        {
            renderers.Add(item);
        }

        //初期位置のバックアップ
        startPos = transform.localPosition;

        isHide = false;
    }

    void Update()
    {
        if (player == null) return;

        AutoCameraControl();
        //Clarity();
        //PlayerHide();
    }

    /// <summary>
    /// ＊壁抜け・床抜け防止
    /// </summary>
    void AutoCameraControl()
    {
        RaycastHit hit;

        //プレイヤーの位置からカメラにレイを飛ばし、ビルと床に衝突したら
        if (Physics.Linecast(root.position + Vector3.up, transform.position, out hit, LayerMask.GetMask("Building")))
        {
            //レイの当たった場所がカメラの位置へ
            transform.position =
                Vector3.Lerp(transform.position, hit.point, recoverySpeed * Time.deltaTime);
        }

        //当たっていなければ本来の位置へ戻る
        else
        {
            transform.localPosition =
                Vector3.Lerp(transform.localPosition, startPos, recoverySpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// プレイヤーとの距離が近くなったら、プレイヤーを透過する
    /// </summary>
    void Clarity()
    {
        float dir = (player.transform.position - transform.position).sqrMagnitude;
        if (dir <= 30) isHide = true;
        else isHide = false;
    }

    /// <summary>
    /// プレイヤーの透過のオンオフ
    /// </summary>
    public void PlayerHide(bool hide)
    {
        if (hide)
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                foreach (var item in renderers)
                {
                    item[i].material.SetFloat("_Mode", 2);
                    item[i].material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    item[i].material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    item[i].material.SetInt("_ZWrite", 0);
                    item[i].material.DisableKeyword("_ALPHATEST_ON");
                    item[i].material.EnableKeyword("_ALPHABLEND_ON");
                    item[i].material.DisableKeyword("_ALPHAPREMULTIPLY_ON");

                    item[i].material.SetOverrideTag("RenderType", "Transparent");
                    item[i].material.renderQueue = 0;
                }
            }
        }
        else
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                foreach (var item in renderers)
                {
                    item[i].material.SetFloat("_Mode", 2);
                    item[i].material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    item[i].material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    item[i].material.SetInt("_ZWrite", 0);
                    item[i].material.DisableKeyword("_ALPHATEST_ON");
                    item[i].material.EnableKeyword("_ALPHABLEND_ON");
                    item[i].material.DisableKeyword("_ALPHAPREMULTIPLY_ON");

                    item[i].material.SetOverrideTag("RenderType", "Transparent");
                    item[i].material.renderQueue = 3000;
                }
            }
        }
    }
}