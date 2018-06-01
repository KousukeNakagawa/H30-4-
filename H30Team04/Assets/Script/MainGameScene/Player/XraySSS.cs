using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class XraySSS : MonoBehaviour
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

    //各々の射影機との距離を近い順に保管するリスト
    SortedList<float, GameObject> _sortXrayDistance = new SortedList<float, GameObject>();

    //射影機との距離を鍵に射影機を持つ
    Dictionary<int, GameObject> _Xrays = new Dictionary<int, GameObject>();
    //全ての射影機との距離のリスト
    List<int> _XrayDistance = new List<int>();


    List<KeyValuePair<int, GameObject>> _XrayList = new List<KeyValuePair<int, GameObject>>();


    //現在示している射影機と前のフレームで示していた射影機
    GameObject _currentXray, _pastXray;

    //ロックオンしている射影機
    GameObject _target;

    GameObject player;
    PlayerBase playerBase;

    //選択している射影機のナンバー（数字で管理）
    int _XrayNum = 0; //０が最も近いインデックス

    void Start()
    {
        //矢印の取得
        _arrow = arrow._lineObject.GetComponent<LineRenderer>();

        Search();

        player = GameObject.FindGameObjectWithTag("Player");
        playerBase = player.GetComponent<PlayerBase>();
    }

    void Update()
    {
        if (!playerBase.GetIsEndSE()) return;

        //Search();

        SSS(); //射影機の「Search」「Select」「Shutter」
    }

    /// <summary>
    /// ＊射影機の「Search」「Select」「Shutter」
    /// </summary>
    void SSS()
    {
        //SelectChange();

        ////射影機の「探索」
        //Search();

        //if (_Xrays.Count == 0) Destroy(_arrow);

        ////射影機の「選択」
        //Select();
        ////射影機の「起動」
        //Shutter();
    }

    /// <summary>
    /// 射影機の検索
    /// </summary>
    void Search()
    {
        //全ての射影機を取得
        var Xrays = GameObject.FindGameObjectsWithTag("Xline");

        //射影機との距離を取得
        foreach (var Xray in Xrays)
            _XrayDistance.Add((int)Vector3.Distance(transform.position, Xray.transform.Find("model").position));

        int r = Xrays.Length;
        int s = _XrayDistance.Count;
        int count = r;

        //全ての値を比較し被りが無いようにする
        for (int i = 0; i < Xrays.Length - 1; i++)
            for (int j = i + 1; j < Xrays.Length; j++)
                if (_XrayDistance[i] == _XrayDistance[j]) _XrayDistance[j] += 1;

        //射影機とその射影機との距離のリンク
        foreach (var key in _XrayDistance)
            foreach (var value in Xrays)
                //どっちも入ってなければ追加
                if (!_Xrays.ContainsKey(key) && !_Xrays.ContainsValue(value))
                    _Xrays.Add(key, value);
                //
                else if (_Xrays.ContainsValue(value))
                    _Xrays.Remove(key);

        //距離をソートしたため、_Xraysもソートされる
        //_XrayDistance.Sort();

        var _XrayList = new List<KeyValuePair<int, GameObject>>(_Xrays);
        //_XrayList.AddRange(_Xrays); 
        //ソート
        _XrayList.Sort((a, b) => a.Key.CompareTo(b.Key));

        //Debug.Log(list[_XrayNum]);
        //for (int i = 0; i < _XrayList.Count; i++)
        //    Debug.Log(_XrayList[i]);

        //射影機の数
        //Debug.Log(Xrays.Length);

        //射影機///距離///順番
        //foreach (var distance in _XrayDistance)
        //    Debug.Log("射影機::" + _Xrays[distance] + "//距離::" + distance + "//順番::" + distance);

        //Debug.Log(_Xrays[_XrayKeys[0]] + "////" + _XrayKeys[0]);

        //foreach (var u in _Xrays.Keys)
        //    Debug.Log(_Xrays[u] + ":::" + u);
    }

    GameObject GetXray(int num)
    {
        var test = new List<GameObject>(_Xrays.Values);
        foreach (var te in test)
        {
            var di = (int)Vector3.Distance(transform.position, te.transform.position);
            if (di == _XrayDistance[num]) return te;
        }

        return null;
    }

    /// <summary>
    /// ＊射影機の「選択」
    /// </summary>
    void Select()
    {
        //現在選択中の射影機のセット
        _target = _Xrays[_XrayDistance[_XrayNum]];
        //射影機の方向
        Vector3 direction = _target.transform.Find("model").position;
        //矢印の始点
        Vector3 start = transform.position + direction * arrow._startPoint / 100 + Vector3.up;
        //矢印の終点
        Vector3 end = direction / 10;

        //距離に比例する透明度
        float depthCalculation = arrow._colorDepth - direction.sqrMagnitude / 10000;
        float depth = (depthCalculation <= arrow._minDepth) ? arrow._minDepth : depthCalculation;

        //選択している射影機と自信を結ぶ線
        DrawArrow(start, start + end, arrow._startColor, arrow._endColor * depth, 2);
    }

    /// <summary>
    /// 選択中の射影機の方角を指す矢印の描画
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

    /// <summary>
    /// 選択中の射影機の取得
    /// </summary>
    public GameObject GetTargetXray()
    {
        return _target;
    }
}