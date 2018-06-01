using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XrayTest : MonoBehaviour
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

        [Range(0.1f, 3)] public float width = 2;

        //近いほど濃くなり、遠いほど薄くなる
        [Range(1, 2), Header("[距離に比例する透明度]")] public float _colorDepth = 1;
        [Range(0, 1)] public float _minDepth = 0.5f;
    }

    [SerializeField] Arrow arrow;
    //射影機の方向を示す矢印
    LineRenderer _arrow;

    //開始演出終了フラグ取得
    GameObject _player;
    PlayerBase _playerBase;
    //XrayManager取得
    [SerializeField] GameObject _XrayManager;
    XrayManager _manager;

    //現在指している射影機・前フレーム指していた射影機
    //一番近い射影機・二番目に近い射影機
    GameObject _currentXray, _oldXray, _Xray1, _Xray2;

    //射影機の選択切替フラグ
    bool _isNear = true;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerBase = _player.GetComponent<PlayerBase>();
        _manager = _XrayManager.GetComponent<XrayManager>();
        _arrow = arrow._lineObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        //開始演出が終わらなければ動かない
        if (!_playerBase.GetIsEndSE()) return;
        //射影機が無ければ終了
        if (_manager.GetXrays().Count == 0) return;

        Search();
        Shutter();
    }

    void Search()
    {
        var _Xrays = _manager.GetXrays();

        //null合体演算子
        _oldXray = _oldXray ?? _Xrays[0].Key;

        //一番近い射影機の取得
        _Xray1 = _Xrays[0].Key;

        //射影機が二つ以上あるなら二番目に近い射影機の取得
        if (_Xrays.Count >= 2)
            _Xray2 = _Xrays[1].Key;

        //示す射影機の切替
        if (Input.GetButtonDown("Select")) _isNear = !_isNear;

        //一番近い射影機を示す
        if (_isNear) _currentXray = _Xray1;
        //二番目に近い射影機を示す
        else if (_Xrays.Count >= 2) _currentXray = _Xray2;

        var selectXray = _currentXray;

        //自動切替防止処理
        //対象が切り替わったとき
        if (_currentXray != _oldXray && !Input.GetButtonDown("Select"))
        {
            _isNear = !_isNear;
            selectXray = _oldXray;
        }
        //前フレームの情報を取得
        else _oldXray = _currentXray;

        Show(selectXray);
    }

    /// <summary>
    /// 選択中の射影機を示す矢印の描画
    /// </summary>
    void Show(GameObject xray)
    {
        //示す射影機の方向
        Vector3 direction = xray.transform./*Find("model").*/position - transform.position;
        //矢印の始点
        Vector3 start = transform.position + direction * arrow._startPoint / 100 + Vector3.up;
        //矢印の終点
        Vector3 end = start + direction / 10;

        //距離に比例する透明度
        float depthCalculation = arrow._colorDepth - direction.sqrMagnitude / 10000;
        float depth = (depthCalculation <= arrow._minDepth) ? arrow._minDepth : depthCalculation;

        DrawArrow(start, end, arrow._startColor, arrow._endColor * depth, arrow.width);
    }

    /// <summary>
    /// 示している射影機の起動
    /// </summary>
    void Shutter()
    {
        if (Input.GetButtonDown("Shutter")) //選択中の射影機の起動
        {
            //_currentCamera.GetComponent<XrayMachine>().XrayPlay();
            _currentXray.GetComponent<XraySupport>().Shutter();
            //_currentXray.SetActive(false);
        }
    }

    /// <summary>
    /// 射影機の方向を指す矢印の描画
    /// </summary>
    void DrawArrow(Vector3 p1, Vector3 p2, Color c1, Color c2, float width, bool isSharp = true)
    {
        if (_manager.GetXrays().Count == 0) Destroy(_arrow);
        _arrow.SetPosition(0, p1);
        _arrow.SetPosition(1, p2);
        _arrow.startColor = c1;
        _arrow.endColor = c2;
        _arrow.startWidth = width;
        _arrow.endWidth = (isSharp) ? 0 : width;
    }

    /// <summary>
    /// 現在示している射影機の取得
    /// </summary>
    public GameObject GetTarget()
    {
        return _currentXray;
    }
}