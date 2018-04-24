using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    //移動スピード
    [SerializeField] [Range(0.1f, 10)] float recoverySpeed = 3f;

    Vector3 startPos;
    Transform root;

    void Start()
    {
        //CameraControllerの情報
        root = transform.root;

        //初期位置のバックアップ
        startPos = transform.localPosition;
    }

    void Update()
    {
        AutoCameraControl(); //壁抜けと床抜けの防止
    }

    /// <summary>
    /// ＊壁抜け・床抜け防止
    /// </summary>
    void AutoCameraControl()
    {
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
}