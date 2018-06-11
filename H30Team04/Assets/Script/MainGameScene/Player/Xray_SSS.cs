using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Xray_SSS : MonoBehaviour
{
    [System.Serializable]
    class Arrow
    {
        //LineRendererを持つオブジェクト
        public GameObject _lineObject;

        //値が小さいほど近くに描画
        [Range(0, 10), Tooltip("矢印描画始点")]
        public int _startPoint = 4;

        //始点・終点の色
        public Color _startColor, _endColor;

        [Range(0.1f, 3)] public float width = 2;

        //近いほど濃くなり、遠いほど薄くなる
        [Range(1, 2), Header("[距離に比例する透明度]")]
        public float _colorDepth = 1;

        //透明度の最低値
        [Range(0, 1)]
        public float _minDepth = 0.5f;
    }

    [SerializeField] GameObject XrayArrow;
    [SerializeField, Range(1, 3)] float arrowOrigin = 1;
    [SerializeField, Range(0.1f, 3)] float arrowHeight = 0.1f;

    [SerializeField] MainCamera m_maincamera;

    [SerializeField] GameObject markwe;

    //[SerializeField] Arrow arrow;
    ////射影機の方向を示す矢印
    //LineRenderer _arrow;

    //開始演出終了フラグ取得
    GameObject _player;
    PlayerBase _playerBase;

    //射影機取得用
    GameObject[] _XrayMachines;

    //射影機との距離を取得し、値をいじるため
    Dictionary<GameObject, float>
        _Xrays = new Dictionary<GameObject, float>();

    //ソートするため
    List<KeyValuePair<GameObject, float>>
        _sortXrays = new List<KeyValuePair<GameObject, float>>();

    //現在指している射影機・前フレーム指していた射影機
    //一番近い射影機・二番目に近い射影機
    GameObject _currentXray, _oldXray, _Xray1, _Xray2;

    GameObject _selectXray;

    //射影機の選択切替フラグ
    bool _isNear = true;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerBase = _player.GetComponent<PlayerBase>();
        //_arrow = arrow._lineObject.GetComponent<LineRenderer>();

        Serch();
    }

    void Update()
    {
        Debug.Log(_currentXray);
        //開始演出が終わらなければ動かない
        //if (!_playerBase.GetIsEndSE()) return;
        //射影機が無ければ終了
        if (_XrayMachines.Length < 1)
        {
            Destroy(XrayArrow);
        }
        else
        {
            //射影機の情報の更新
            XraysUpdate();

            Select();
            Shutter();
        }
    }

    /// <summary>
    /// 射影機の情報の取得
    /// </summary>
    void Serch()
    {
        //射影機の取得
        _XrayMachines = XrayMachines.xrayMachineObjects.ToArray();

        foreach (var machine in _XrayMachines)
        {
            //自身とプレイヤーとの距離を取得
            var distance = Vector3.Distance(transform.position, machine.transform.position);
            distance = Mathf.Round(distance);

            //距離が重複しないように調整
            if (_Xrays.ContainsValue(distance)) distance += 0.1f;

            _Xrays.Add(machine, distance);
        }
        ListUpdate();
    }

    /// <summary>
    /// 示している射影機の選択
    /// </summary>
    void Select()
    {
        if (_XrayMachines.Length < 1)
        {
            markwe.SetActive(false);
            return;
        }

        //null演算子
        _oldXray = _oldXray ?? _sortXrays[0].Key;

        //一番近い射影機の取得
        if (_sortXrays.Count != 0)
        {
            _Xray1 = _sortXrays[0].Key;
        }

        //射影機が二つ以上あるなら二番目に近い射影機の取得
        if (_Xrays.Count >= 2) _Xray2 = _sortXrays[1].Key;

        //示す射影機の切替
        if (Input.GetButtonDown("Select"))
        {
            _isNear = !_isNear;
        }

        //一番近い射影機を示す
        if (_isNear) _currentXray = _Xray1;
        //二番目に近い射影機を示す
        else if (_Xrays.Count >= 2) _currentXray = _Xray2;

        //矢印の示す射影機
        _selectXray = _currentXray; //_current

        //対象が切り替わったとき
        if (_currentXray != _oldXray && !Input.GetButtonDown("Select"))
        {
            _isNear = !_isNear;
            _selectXray = _oldXray;
        }
        //前フレームの情報を取得
        else _oldXray = _currentXray;

        if (_selectXray.ToString() == "null")
        {
            //示している射影機が消えたら
            if (_isNear) _selectXray = _Xray2;
            else _selectXray = _Xray1;
            return;
        }

        Marker();
        Show();
    }

    /// <summary>
    /// 選択中の射影機を示す矢印の描画
    /// </summary>
    void Show()
    {
        XrayArrow.SetActive(UnlockManager.limit[UnlockState.xray]);

        //示す射影機の方向
        Vector3 direction = _selectXray.transform.Find("model").position - transform.position;
        ////矢印の始点
        //Vector3 start = transform.position + direction / 100 * arrow._startPoint + Vector3.up;
        ////矢印の終点
        //Vector3 end = start + direction/* / 10*/;

        ////距離に比例する透明度
        //float depthCalculation = arrow._colorDepth - direction.sqrMagnitude / 10000;
        //float depth = (depthCalculation <= arrow._minDepth) ? arrow._minDepth : depthCalculation;

        Vector3 arrowPos = transform.position + direction.normalized * arrowOrigin + Vector3.up * arrowHeight;
        XrayArrow.transform.position = arrowPos;
        XrayArrow.transform.LookAt(_selectXray.transform.Find("model").position);
        XrayArrow.transform.eulerAngles = new Vector3(90, XrayArrow.transform.eulerAngles.y, XrayArrow.transform.eulerAngles.z);

        //DrawArrow(start, end, arrow._startColor, arrow._endColor * depth, arrow.width);
    }

    void Marker()
    {
        Vector3 m_target = _selectXray.transform.Find("model").position;
        markwe.transform.LookAt(m_maincamera.transform.transform.position);

        markwe.transform.position = new Vector3(m_target.x, m_target.y + 5, m_target.z);
    }

    /// <summary>
    /// 示している射影機の起動
    /// </summary>
    void Shutter()
    {
        if (Input.GetButtonDown("Shutter")) //選択中の射影機の起動
        {
            _selectXray.GetComponent<XrayMachine>().XrayPlay();
        }
    }

    /// <summary>
    /// 射影機の情報の更新
    /// </summary>
    void ListUpdate()
    {
        //距離を比較し、重複を解除する
        //for (int i = 0; i < _XrayMachines.Length - 1; i++)
        //    for (int j = i + 1; j < _XrayMachines.Length; j++)
        //        if (_Xrays[_sortXrays[i].Key] == _Xrays[_sortXrays[j].Key])
        //            _Xrays[_sortXrays[j].Key] += 0.1f;

        //リスト化
        _sortXrays = _Xrays.ToList();
        //距離を元にソート
        _sortXrays.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
    }

    /// <summary>
    /// 射影機の情報の更新
    /// </summary>
    void XraysUpdate()
    {
        _Xrays.Clear();

        Serch();
    }

    ///// <summary>
    ///// 射影機の方向を指す矢印の描画
    ///// </summary>
    //void DrawArrow(Vector3 p1, Vector3 p2, Color c1, Color c2, float width, bool isSharp = true)
    //{
    //    if (_Xrays.Count == 0) Destroy(_arrow);
    //    _arrow.SetPosition(0, p1);
    //    _arrow.SetPosition(1, p2);
    //    _arrow.startColor = c1;
    //    _arrow.endColor = c2;
    //    _arrow.startWidth = width;
    //    _arrow.endWidth = (isSharp) ? 0 : width;
    //}

    /// <summary>
    /// 現在示している射影機の取得
    /// </summary>
    public GameObject GetTarget()
    {
        return _currentXray;
    }
}