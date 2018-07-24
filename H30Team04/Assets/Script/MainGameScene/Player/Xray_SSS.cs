using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Xray_SSS : MonoBehaviour
{
    /*** 必要ゲームオブジェクト ***/
    /// <summary> 選択中の射影機を示す矢印（通常） </summary>
    [SerializeField] GameObject blueXrayArrow;
    /// <summary> 選択中の射影機を示す矢印（ロボ撮影可） </summary>
    [SerializeField] GameObject yellowXrayArrow;
    /// <summary> 選択中の射影機を示す矢印 </summary>
    GameObject XrayArrow;
    /// <summary> カメラ </summary>
    [SerializeField] MainCamera m_maincamera;
    /// <summary> マーカー </summary>
    [SerializeField] GameObject markwe;
    /// <summary> 撮影UI </summary>
    [SerializeField] GameObject shutterStringUI;

    /*** 矢印情報 ***/
    /// <summary> 矢印の始点 </summary>
    [SerializeField, Range(1, 3), Tooltip("矢印の開始位置")] float arrowOrigin = 1;
    /// <summary> 矢印の高さ </summary>
    [SerializeField, Range(0.1f, 3), Tooltip("矢印の高さ")] float arrowHeight = 0.1f;

    /*** 射影機情報 ***/
    /// <summary> 選択中の射影機 </summary>
    GameObject _selectXray;
    /// <summary> 現在選択中の射影機 </summary>
    GameObject _currentXray;
    /// <summary> 前フレームに選択していた射影機 </summary>
    GameObject _oldXray;
    /// <summary> 一番近い射影機 </summary>
    GameObject _Xray1;
    /// <summary> 二番目に近い射影機 </summary>
    GameObject _Xray2;

    /// <summary> 全射影機取得 </summary>
    GameObject[] _XrayMachines;
    /// <summary> 距離を被りなく取得する</summary>
    Dictionary<GameObject, float>
        _Xrays = new Dictionary<GameObject, float>();
    /// <summary> 距離をソートする </summary>
    List<KeyValuePair<GameObject, float>>
        _sortXrays = new List<KeyValuePair<GameObject, float>>();

    /// <summary> 射影機目線になるときの位置 </summary>
    public static Vector3 ShutterPos;
    /// <summary> 射影機目線になるときの向き </summary>
    public static Vector3 ShutterAngle;
    /// <summary> 射影機の切替フラグ </summary>
    bool _isNear = true;
    /// <summary> 現在選択中の射影機の生存確認用 </summary>
    bool currentSelectXrayState;
    /// <summary> 前フレームに選択していた射影機の生存確認用 </summary>
    bool oldSelectXrayState;
    /// <summary> LTを押しているか </summary>
    bool isLTPermission = true;
    /// <summary> 現在のLTの状態 </summary>
    float currentLt;
    /// <summary> 前フレームのLTの状態 </summary>
    float oldLT;
    /// <summary> LTを押した瞬間を取得するフラグ </summary>
    bool downLT = false;

    /*** プロパティ ***/

    /// <summary> LTを押しているか </summary>
    public static bool IsShutterChance { get; private set; }

    void Start()
    {
        // 初回射影機検索
        Serch();
        IsShutterChance = false;
    }

    void Update()
    {
        // 一時停止中は何もしない
        if (Time.timeScale == 0) return;

        // 射影機がなくなったらUIと矢印の削除
        if (_XrayMachines.Length < 1)
        {
            Destroy(shutterStringUI);
            Destroy(XrayArrow);
        }

        // [xray]がアンロックされていないなら何もしない
        if (!UnlockManager.Limiter[UnlockState.xray]) return;

        //射影機が無くなったら矢印を消す
        if (_XrayMachines.Length > 0)
        {
            // 射影機の更新
            XraysUpdate();
            // 射影機の選択
            Select();
            // 射影機の起動
            Shutter();
        }
    }

    /// <summary> 射影機の検索 </summary>
    void Serch()
    {
        // 全射影機を取得
        _XrayMachines = XrayMachines.xrayMachineObjects.ToArray();

        foreach (var machine in _XrayMachines)
        {
            // 自身とそれぞれの射影機との距離
            var distance = Vector3.Distance(transform.position, machine.transform.position);
            distance = Mathf.Round(distance);

            //距離が重複しないように調整
            if (_Xrays.ContainsValue(distance)) distance += 0.1f;

            _Xrays.Add(machine, distance);
        }
        // 距離の更新
        DistanceUpdate();
    }

    /// <summary> 射影機の選択 </summary>
    void Select()
    {
        // 初回のみ実施
        _oldXray = _oldXray ?? _sortXrays[0].Key;

        // 射影機がなくなったらマーカーの表示を消し、終了
        if (_XrayMachines.Length < 1)
        {
            markwe.SetActive(false);
            return;
        }

        // 選択中の射影機が存在しているかを取得・更新
        currentSelectXrayState = XrayMachines.DeadOrAlive(_selectXray);

        //一番近い射影機の取得
        if (_sortXrays.Count != 0) _Xray1 = _sortXrays[0].Key;
        //射影機が二つ以上あるなら二番目に近い射影機の取得
        if (_Xrays.Count >= 2) _Xray2 = _sortXrays[1].Key;

        // Yボタンを押した瞬間
        if (Input.GetButtonDown("newXrayChange"))
        {
            if (IsShutterChance) return;
            GameTextController.TextStart(5);
            // 対象を切り替える
            _isNear = !_isNear;
        }
        // 選択中の射影機が消失、前フレームは生きていたら
        // 何もしない
        if (!currentSelectXrayState && oldSelectXrayState) { }

        //一番近い射影機を示す
        if (_isNear) _currentXray = _Xray1;
        //二番目に近い射影機を示す
        else if (_Xrays.Count >= 2) _currentXray = _Xray2;

        // 射影機選択の補助
        SelectSupport();

        // 選択中の射影機が消えたら
        if (_selectXray.ToString() == "null")
        {
            // 別の射影機に切り替える
            if (_isNear) _selectXray = _Xray2;
            else _selectXray = _Xray1;
            return;
        }

        // 選択中の射影機の取得
        SetSelectXray();

        // マーカーの処理
        Marker();
        // 選択中の射影機の矢印の描画
        Show();
        // 選択中の射影機がいなくなったら切り替える処理
        Research();
    }

    /// <summary> 射影機選択の補助 </summary>
    void SelectSupport()
    {

        /*** 切り替わった対象を戻す処理 ***/
        /*** 意図的に切り替えたなら無視する ***/

        //矢印の示す射影機の更新
        _selectXray = _currentXray;
        // 初回のみ実施
        XrayArrow = XrayArrow ?? blueXrayArrow;

        // 矢印の色のチェンジ
        XrayArrow = (_selectXray.GetComponent<XrayMachine>().IsWeakFrontBuil()) ?
            yellowXrayArrow : blueXrayArrow;

        // 選択中の射影機が弱点を写したら 黄色にする それ以外は常時青
        yellowXrayArrow.SetActive(_selectXray.GetComponent<XrayMachine>().IsWeakFrontBuil());
        blueXrayArrow.SetActive(!_selectXray.GetComponent<XrayMachine>().IsWeakFrontBuil());
        // UIの表示
        shutterStringUI.SetActive(_selectXray.GetComponent<XrayMachine>().IsWeakFrontBuil());
        // [xray]がアンロックされているなら矢印の描画
        XrayArrow.SetActive(UnlockManager.Limiter[UnlockState.xray]);

        // 切替ボタンを押していないのに 示している射影機が切り替わった瞬間
        if (!Input.GetButtonDown("newXrayChange") && _currentXray != _oldXray)
        {
            // 切り替わる前に戻す
            _isNear = !_isNear;
            // 切り替わる前の射影機を示し続ける
            _selectXray = _oldXray;
        }
        // 切り替わっていないなら
        // 前フレーム示していた射影機を更新
        else _oldXray = _currentXray;

        // 選択中の射影機が消失している & 前フレームは存在していた <= 意図的に切り替える条件
        // 選択中の射影機が存在している & 前フレームは消失していた <= 無視させたい条件
        // 無視させたい条件中に 示している射影機が切り替わった瞬間
        if ((currentSelectXrayState || !oldSelectXrayState) && _currentXray != _oldXray)
        {
            //切り替わる前に戻す
            _isNear = !_isNear;
            //切り替わる前の射影機を示し続ける
            _selectXray = _oldXray;
        }
        //切り替わっていないなら
        else
        {
            // 前フレーム示していた射影機を更新
            _oldXray = _currentXray;
        }
        // 選択中の射影機の前フレームの状態を更新
        oldSelectXrayState = currentSelectXrayState;
    }

    /// <summary> 選択中の射影機の「ShutterPos」情報のセッティング </summary>
    void SetSelectXray()
    {
        // 選択中の射影機が持つ「ShutterPos」の位置と角度を取得
        ShutterPos = _selectXray.transform.Find("ShutterPos").position;
        ShutterAngle = ShutterPos + _selectXray.transform.Find("ShutterPos").forward;
    }

    /// <summary> 再検索 </summary>
    void Research()
    {
        // 選択中の射影機を起動させたら 切り替える
        if (!_selectXray.CompareTag("Xline")) _isNear = !_isNear;
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

        // 矢印の位置
        var arrowPos = transform.position + direction.normalized * arrowOrigin + Vector3.up * arrowHeight;
        XrayArrow.transform.position = arrowPos;
        // 選択中の射影機を指す
        XrayArrow.transform.LookAt(_selectXray.transform.position);
        XrayArrow.transform.eulerAngles = new Vector3(90, XrayArrow.transform.eulerAngles.y, XrayArrow.transform.eulerAngles.z);

        //DrawArrow(start, end, arrow._startColor, arrow._endColor * depth, arrow.width);
    }

    /// <summary> マーカーの描画 </summary>
    void Marker()
    {
        Vector3 m_target = _selectXray.transform.position;

        markwe.transform.position = new Vector3(m_target.x, m_target.y + 5, m_target.z);
        markwe.transform.LookAt(m_maincamera.transform.transform.position);
    }

    /// <summary> 射影機の起動 </summary>
    void Shutter()
    {
        // LTの状態を更新
        currentLt = Input.GetAxis("ShutterChance");

        // 基本 false 押した瞬間 true
        if (currentLt > oldLT) downLT = true;
        // 戻っている間 false
        if (currentLt < oldLT) downLT = false;

        // 基本 true
        isLTPermission = true;

        // LTを押したら
        if (IsShutterChance)
        {
            // 選択中の射影機が壊された判定になったら
            if (_selectXray.GetComponent<XrayMachine>().isBreak)
                // LTを解除する
                isLTPermission = false;
        }

        // LTを押している間 選択中の射影機が使用可能な場合 カメラが戻っている最中ではないなら
        IsShutterChance = (Input.GetAxis("ShutterChance") > 0 && 
            !MainCamera.IsComeBack && isLTPermission && downLT);

        // Xボタンを押したら
        if (Input.GetButtonDown("newShutter"))
        {
            // 博士のセリフ
            GameTextController.TextStart(4);
            // 射影機の起動
            _selectXray.GetComponent<XrayMachine>().XrayPlay();
            // 一番近いものを見る
            _isNear = true;
        }

        // 視点移動中は保持している射影機を消さない処理
        if (Input.GetAxis("ShutterChance") <= 0)
            _selectXray.GetComponent<XrayMachine>().XrayPlaySupport();

        // 前フレームの状態を更新
        oldLT = currentLt;
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
        // 全消し
        _Xrays.Clear();
        // 取得
        Serch();
    }

    /// <summary> 選択中の射影機の取得 </summary>
    public GameObject GetTarget()
    {
        return _selectXray;
    }

    /// <summary> UIの描画 </summary>
    void DrawShutterUI()
    {

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