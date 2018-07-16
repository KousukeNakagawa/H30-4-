using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TutorialXray_SSS : MonoBehaviour {

    [SerializeField] GameObject XrayArrow;
    [SerializeField, Range(1, 3), Tooltip("矢印の開始位置")] float arrowOrigin = 1;
    [SerializeField, Range(0.1f, 3), Tooltip("矢印の高さ")] float arrowHeight = 0.1f;

    [SerializeField] TutorialMainCamera m_maincamera;
    [SerializeField] GameObject markwe;

    public TutorialManager_T tmane;

    public GameObject[] nextXray;
    
    /// <summary> 選択中の射影機 </summary>
    public GameObject _selectXray;
    /// <summary> 射影機目線になるか </summary>
    public static bool IsShutterChance { get;  set; }
    /// <summary> 射影機目線になるときの位置 </summary>
    public static Vector3 ShutterPos;
    /// <summary> 射影機目線になるときの向き </summary>
    public static Vector3 ShutterAngle;
    /// <summary> 現在選択中の射影機の生存確認用 </summary>
    bool currentSelectXrayState, oldSelectXrayState;

    private int pushCount = 0;

    void Start()
    {
        IsShutterChance = false;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        bool arrowActiv = tmane.GetState() >= TutorialState_T.CURSOR;
        //if (!tmane.IsReaded()) arrowActiv = false;
        XrayArrow.SetActive(arrowActiv);
        //if (!tmane.IsReaded() || tmane.GetState() < TutorialState_T.SHUTTER) return;

        Select();
            Shutter();
    }
    

    /// <summary> 射影機の選択 </summary>
    void Select()
    {
        if (tmane.IsReaded() && tmane.GetState() == TutorialState_T.CURSORCHANGE)
        {
            if (Input.GetButtonDown("newXrayChange"))
            {
                _selectXray = nextXray[0];
            }
        }
        SetSelectXray();

        Marker();
        Show();
    }
    

    /// <summary> 選択中の射影機の「ShutterPos」情報のセッティング </summary>
    void SetSelectXray()
    {
        ShutterPos = _selectXray.transform.Find("ShutterPos").position;
        ShutterAngle = ShutterPos + _selectXray.transform.Find("ShutterPos").forward;
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
        if (!tmane.IsReaded() || tmane.GetState() !=TutorialState_T.SHUTTER)
        {
            IsShutterChance = false;
            return;
        }

        //射影機目線になる
        IsShutterChance = (Input.GetAxis("ShutterChance") > 0 && XrayMachines.DeadOrAlive(_selectXray));

        if (IsShutterChance&&Input.GetButtonDown("newShutter"))
        {
            //テキスト送りと同時に進んでほしくないから苦し紛れ
            //if (pushCount == 0)
            //{
            //    pushCount++;
            //    return;
            //}
            _selectXray.GetComponent<TutorialXrayMachine>().XrayPlay();
            _selectXray = nextXray[1];
        }

        // 視点移動中は保持している射影機を消さない処理
        if (Input.GetAxis("ShutterChance") <= 0) _selectXray.GetComponent<TutorialXrayMachine>().XrayPlaySupport();
    }
    
    /// <summary> 選択中の射影機の取得 </summary>
    public GameObject GetTarget()
    {
        return _selectXray;
    }
}
