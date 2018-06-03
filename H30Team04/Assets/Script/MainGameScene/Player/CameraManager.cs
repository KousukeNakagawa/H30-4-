using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> カメラの切替 </summary>
public class CameraManager : MonoBehaviour
{
    List<GameObject> cameras = new List<GameObject>();
    [SerializeField] GameObject SE_camera;
    [SerializeField] GameObject playerCamera;

    void Start()
    {
        //リスト化
        cameras.Add(SE_camera);
        cameras.Add(playerCamera);

        SetActiveCamera(SE_camera);
    }

    void Update()
    {
        //開始演出終了時
        if (SEManager.IsEndSE)
        {
            SetActiveCamera(playerCamera);
            Destroy(gameObject);
        }
    }

    /// <summary> 使用するカメラの選択 </summary>
    void SetActiveCamera(GameObject activCamera)
    {
        //選択されたカメラを表示
        activCamera.SetActive(true);

        //選択されなかったカメラを全て非表示
        foreach (var camera in cameras)
            if (camera != activCamera) camera.SetActive(false);
    }
}
