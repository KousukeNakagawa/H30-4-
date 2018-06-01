using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player

public class Xray_S_S_S : MonoBehaviour
{
    [System.Serializable]
    class Arrow
    {
        //LineRendererを持つオブジェクト
        public GameObject _lineObject;

        //値が小さいほど近くに描画
        [Range(0, 10), Tooltip("矢印描画始点")] public int _startPoint = 4;

        //始点・終点の色
        public Color _startColor, _endColor;

        //近いほど濃くなり、遠いほど薄くなる
        [Range(1, 2), Header("[距離に比例する透明度]")] public float _colorDepth = 1;
        [Range(0, 1)] public float _minDepth = 0.5f;
    }

    [SerializeField, Tooltip("最大捕捉数")] int _maxCapture = 2;
    [SerializeField] Arrow arrow;

    //射影機の方向を示す矢印
    LineRenderer _arrow;

    int num = 0;
    bool _isNear = true;



    [SerializeField] GameObject _XrayManager;
    XrayManager _manager;
    GameObject _player;
    PlayerBase _playerBase;

    GameObject _currentCamera;
    GameObject _camera1;
    GameObject _camera2;

    GameObject _oldCurrentCamera;
    GameObject _oldCamera1;
    GameObject _oldCamera2;

    //GameObject _lockedOnCamera;

    private void Reset()
    {
        //_XrayManager = GetComponent<XrayManager>();

    }

    //TestCamera lockedOnCamera = null;

    void Start()
    {
        _manager = _XrayManager.GetComponent<XrayManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerBase = _player.GetComponent<PlayerBase>();
        _arrow = arrow._lineObject.GetComponent<LineRenderer>();
        //_lockedOnCamera = null;
    }

    void Update()
    {
        if (!_playerBase.GetIsEndSE()) return;

        Search();
        Select();
        Shutter();
    }

    void Search()
    {
        //前のフレームの情報の取得
        _oldCamera1 = _camera1;
        _oldCamera2 = _camera2;
        _oldCurrentCamera = _currentCamera;

        //ソートされたDictionary＜射影機・距離＞の取得
        var sortedCameraDict = _manager.GetXrays();

        if (_isNear)
        {
            //一番近い射影機の取得
            _camera1 = sortedCameraDict[num].Key;
            //二番目に近い射影機の取得
            _camera2 = sortedCameraDict[num + 1].Key;
        }
        else
        {
            //一番近い射影機の取得
            _camera1 = sortedCameraDict[num + 1].Key;
            //二番目に近い射影機の取得
            _camera2 = sortedCameraDict[num].Key;
        }

        //今と前で、一番目と二番目に近い射影機、どちらかが一緒なら
        if (_camera1 == _oldCamera1 || _camera1 == _oldCamera2)
        {
            _currentCamera = _oldCurrentCamera;
        }

        //二番目に近いカメラが前一番近かったカメラなら・前に番目に近かったカメラなら
        if (_camera2 != _oldCamera1 && _camera2 != _oldCamera2)
        {
            _currentCamera = _camera1;
        }

        Debug.Log("current::" + _currentCamera + "///old::" + _oldCurrentCamera);
        Debug.Log(sortedCameraDict.Count);


        // if lockOnButton is pressed
        // lockedOnCamera = currentCamera;

        // if cameraSwitch is pressed
        // lockedOnCamera = null;
        // currentCamera = camera1 mataha camera2

        // if lockedOnCamera == null
        // arrow aim currentCamera
        // else
        // arrow aim lockedOnCamera

        //現在選択中の射影機のセット
        //_target = _Xrays[_XrayDistance[_XrayNum]];
    }

    void Select()
    {
        if (Input.GetButtonDown("Select")) _isNear = !_isNear;
        Debug.Log(_isNear);

        //射影機の方向
        Vector3 direction = _currentCamera.transform./*Find("model").*/position;
        //矢印の始点
        Vector3 start = transform.position + direction * arrow._startPoint / 100 + Vector3.up;
        //矢印の終点
        Vector3 end = direction / 10;

        //距離に比例する透明度
        float depthCalculation = arrow._colorDepth - direction.sqrMagnitude / 10000;
        float depth = (depthCalculation <= arrow._minDepth) ? arrow._minDepth : depthCalculation;

        //選択している射影機と自信を結ぶ線
        DrawArrow(start, start + end, arrow._startColor, arrow._endColor * depth, 2);
        //DrawArrow(direction, direction + Vector3.up * 100, arrow._startColor, arrow._endColor, 2);
    }

    void Shutter()
    {
        var sortedCameraDict = _manager.GetXrays();

        if (sortedCameraDict.Count <= 0) return;

        if (Input.GetButtonDown("Shutter")) //選択中の射影機の起動
        {
            //_currentCamera.GetComponent<XrayMachine>().XrayPlay();
            _currentCamera.GetComponent<XraySupport>().Shutter();
        }
    }

    /// <summary>
    /// 射影機の方向を指す矢印の描画
    /// </summary>
    void DrawArrow(Vector3 p1, Vector3 p2, Color c1, Color c2, float width, bool isSharp = true)
    {
        _arrow.SetPosition(0, p1);
        _arrow.SetPosition(1, p2);
        _arrow.startColor = c1;
        _arrow.endColor = c2;
        _arrow.startWidth = width;
        _arrow.endWidth = (isSharp) ? 0 : width;
    }
}
