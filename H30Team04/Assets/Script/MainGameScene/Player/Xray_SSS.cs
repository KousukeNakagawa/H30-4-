using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Xray_SSS : MonoBehaviour
{
    [SerializeField] GameObject XrayArrow;
    [SerializeField, Range(1, 3), Tooltip("矢印の開始位置")] float arrowOrigin = 1;
    [SerializeField, Range(0.1f, 3), Tooltip("矢印の高さ")] float arrowHeight = 0.1f;

    [SerializeField] MainCamera m_maincamera;
    [SerializeField] GameObject markwe;

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
    //選択している射影機
    GameObject _selectXray;

    //射影機の選択切替フラグ
    bool _isNear = true;

    /// <summary> 射影機目線になるか </summary>
    public static bool IsShutterChance { get; private set; }
    public static Vector3 ShutterPos;
    public static Vector3 ShutterAngle;

    void Start()
    {
        IsShutterChance = false;
        Serch();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        XrayArrow.SetActive(UnlockManager.Limiter[UnlockState.xray]);

        if (!UnlockManager.Limiter[UnlockState.xray]) return;


        //射影機が無くなったら矢印を消す
        if (_XrayMachines.Length < 1)
        {
            Destroy(XrayArrow);
        }
        else
        {
            XraysUpdate();
            Select();
            Shutter();
        }
    }

    /// <summary> 射影機の検索 </summary>
    void Serch()
    {
        //射影機情報の取得
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

        DistanceUpdate();
    }

    /// <summary> 射影機の選択 </summary>
    void Select()
    {
        if (_XrayMachines.Length < 1)
        {
            markwe.SetActive(false);
            return;
        }
        //初回のみの処理
        _oldXray = _oldXray ?? _sortXrays[0].Key;

        //一番近い射影機の取得
        if (_sortXrays.Count != 0) _Xray1 = _sortXrays[0].Key;
        //射影機が二つ以上あるなら二番目に近い射影機の取得
        if (_Xrays.Count >= 2) _Xray2 = _sortXrays[1].Key;

        //射影機の切替
        if (Input.GetButtonDown("Select") && !IsShutterChance)
        {
            //GameTextController.TextStart(5);
            _isNear = !_isNear;
        }

        //一番近い射影機を示す
        if (_isNear) _currentXray = _Xray1;
        //二番目に近い射影機を示す
        else if (_Xrays.Count >= 2) _currentXray = _Xray2;

        //矢印の示す射影機の更新
        _selectXray = _currentXray;

        //射影機らとの距離順番が変わってしまった時の処理
        if (_currentXray != _oldXray && !Input.GetButtonDown("Select") && !XrayMachines.DeadOrAlive(_selectXray))
        {
            _isNear = !_isNear;
            _selectXray = _oldXray;
        }
        //前フレームの情報を取得
        else _oldXray = _currentXray;

        #region 射影機が破壊される直前に遺言を貰いたい…

        if (_selectXray.ToString() == "null")
        {
            if (_isNear) _selectXray = _Xray2;
            else _selectXray = _Xray1;
            return;
        }

        #endregion

        ShutterPos = _selectXray.transform.Find("ShutterPos").position;
        ShutterAngle = ShutterPos + _selectXray.transform.Find("ShutterPos").forward;

        Marker();
        Show();
    }

    /// <summary> 矢印の描画 </summary>
    void Show()
    {
        //示す射影機の方向
        Vector3 direction = _selectXray.transform.position - transform.position;
        ////矢印の始点
        //Vector3 start = transform.position + direction / 100 * arrow._startPoint + Vector3.up;
        ////矢印の終点
        //Vector3 end = start + direction/* / 10*/;

        ////距離に比例する透明度
        //float depthCalculation = arrow._colorDepth - direction.sqrMagnitude / 10000;
        //float depth = (depthCalculation <= arrow._minDepth) ? arrow._minDepth : depthCalculation;

        Vector3 arrowPos = transform.position + direction.normalized * arrowOrigin + Vector3.up * arrowHeight;
        XrayArrow.transform.position = arrowPos;
        XrayArrow.transform.LookAt(_selectXray.transform.position);
        XrayArrow.transform.eulerAngles = new Vector3(90, XrayArrow.transform.eulerAngles.y, XrayArrow.transform.eulerAngles.z);

        //DrawArrow(start, end, arrow._startColor, arrow._endColor * depth, arrow.width);
    }

    void Marker()
    {
        Vector3 m_target = _selectXray.transform.position;

        markwe.transform.position = new Vector3(m_target.x, m_target.y + 5, m_target.z);
        markwe.transform.LookAt(m_maincamera.transform.transform.position);
    }

    /// <summary> 射影機の起動 </summary>
    void Shutter()
    {
        //射影機目線になる
        IsShutterChance = (Input.GetAxis("ShutterChance") > 0 && XrayMachines.DeadOrAlive(_selectXray));

        if (Input.GetButtonDown("Shutter"))
        {
            //GameTextController.TextStart(4);
            _selectXray.GetComponent<XrayMachine>().XrayPlay();
        }
    }

    /// <summary> 射影機との距離の更新 </summary>
    void DistanceUpdate()
    {
        //リスト化
        _sortXrays = _Xrays.ToList();
        //距離を元にソート
        _sortXrays.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
    }

    /// <summary> 射影機の情報の更新 </summary>
    void XraysUpdate()
    {
        _Xrays.Clear();
        Serch();
    }

    /// <summary> 選択中の射影機の取得 </summary>
    public GameObject GetTarget()
    {
        return _selectXray;
    }

    #region カス

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

    #endregion
}