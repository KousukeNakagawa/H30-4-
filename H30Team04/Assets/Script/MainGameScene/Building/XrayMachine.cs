using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XrayMachine : MonoBehaviour
{

    private bool xrayOK = true;
    private GameObject m_XrayCameraObj;
    private Camera m_XrayCamera;

    public string texnumber = "01";
    private int weekLayerMask;
    private int builLayerMask;

    private GameManager gameManager;
    [SerializeField] private float saveTime = 10.0f;
    [SerializeField] private float underPos = 100.0f;
    [SerializeField] private GameObject flashPrefab;

    private List<GameObject> weekPoints;

    [SerializeField] Image minimapIcon;
    [SerializeField] Image minimapArrow;
    [SerializeField] private Camera m_MinimapCamera;
    [SerializeField]
    MinimapScript _minimapScript;
    MiniMAPcamera _minimapCS;
    private Rect _canvasRect;
    Rect _rect = new Rect(0, 0, 1, 1);
    RectTransform m_minimap_rect;
    [SerializeField] float y;

    private GameObject m_checkIcon;

    // Use this for initialization
    void Start()
    {
        m_XrayCameraObj = transform.Find("XrayCamera").gameObject;
        m_XrayCamera = m_XrayCameraObj.GetComponent<Camera>();
        m_XrayCamera.targetTexture = Resources.Load("Texture/RenderTextures/XrayCamera" + texnumber) as RenderTexture;
        weekLayerMask = LayerMask.GetMask(new string[] { LayerMask.LayerToName(8) });
        builLayerMask = LayerMask.GetMask(new string[] { LayerMask.LayerToName(9) });

        weekPoints = new List<GameObject>();
        _minimapCS = m_MinimapCamera.GetComponent<MiniMAPcamera>();
        m_minimap_rect = minimapIcon.GetComponent<RectTransform>();
        //saveTime = 10.0f;

        m_checkIcon = transform.Find("checkicon").Find("icon").gameObject;
        m_checkIcon.SetActive(false);

        GameObject gamemanagerObj = GameObject.Find("GameManager");

        if (gamemanagerObj != null) gameManager = gamemanagerObj.GetComponent<GameManager>();

        // UIがはみ出ないようにする
        _canvasRect = ((RectTransform)minimapArrow.canvas.transform).rect;
        _canvasRect.Set(
           _canvasRect.x + minimapArrow.rectTransform.rect.width * 0.5f,
            _canvasRect.y + minimapArrow.rectTransform.rect.height * 0.5f,
            _canvasRect.width - minimapArrow.rectTransform.rect.width,
            _canvasRect.height - minimapArrow.rectTransform.rect.height
        );
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    Shooting();
        //}
        if (!xrayOK && saveTime > 0)
        {
            saveTime -= Time.deltaTime;
            if (saveTime <= 0) //指定時間守り切ったとき、守れなかったときは下記OnCollisionEnterへ
            {
                if (weekPoints.Count != 0)
                {
                    List<int> weeknums = new List<int>();
                    for (int i = 0; i < weekPoints.Count; i++)
                    {
                        weekPoints[i].GetComponent<WeekPoint>().ActiveModel();
                        weeknums.Add(weekPoints[i].GetComponent<WeekPoint>().GetWeekNumber);
                    }
                    SendForManager(weeknums);
                }

                //m_XrayCameraObj.SetActive(false); //使ったカメラは非表示に

            }
        }

        if (!xrayOK) return;

        minimapIcon.rectTransform.rotation = Quaternion.Euler(90, 0, y);
        minimapArrow.rectTransform.rotation = Quaternion.Euler(90, 0, y);

        if (_minimapCS.Pose)
        {
            minimapIcon.transform.position = transform.position;
            m_minimap_rect.sizeDelta = new Vector2(5, 5);
        }
        var viewport = m_MinimapCamera.WorldToViewportPoint(this.transform.position);
        if (_minimapCS.MiniCameraRect.Contains(viewport))
        {
            minimapIcon.transform.position = transform.position;
            minimapIcon.enabled = true;
            minimapArrow.enabled = false;
        }
        else
        {
            minimapIcon.enabled = false;
            minimapArrow.enabled = true;

            //var aim = minimapArrow.transform.position - this.transform.position;
            //var look = Quaternion.LookRotation(aim, minimapArrow.transform.up);
            //minimapArrow.transform.localRotation = look;

            viewport.x = Mathf.Clamp01(viewport.x);
            viewport.y = Mathf.Clamp01(viewport.y);
            minimapArrow.rectTransform.anchoredPosition = Rect.NormalizedToPoint(_canvasRect, viewport);
        }
    }

    //撮影
    private void Shooting()
    {
        //Debug.Log("食らいやがれー");

        //ビルがあるかどうか
        RaycastHit builhit;
        bool hit = Physics.Raycast(
            m_XrayCameraObj.transform.position, m_XrayCameraObj.transform.forward,
            out builhit, m_XrayCamera.farClipPlane, builLayerMask);

        //なかったら終了
        if (!hit)
        {
            if (XrayMachines.xrayMachineObjects.Count <= 1) gameManager.XrayZero();
            GameTextController.FailedText(12);
            return;
        }

        //弱点が写っているかどうか
        RaycastHit[] weekpoints
            = Physics.BoxCastAll(m_XrayCameraObj.transform.position,
            new Vector3(4, m_XrayCameraObj.transform.position.y, 4),
            m_XrayCameraObj.transform.forward, Quaternion.identity, m_XrayCamera.farClipPlane, weekLayerMask);

        //写っていなかったら終了
        if (weekpoints.Length < 1)
        {
            if (XrayMachines.xrayMachineObjects.Count <= 1) gameManager.XrayZero();
            GameTextController.FailedText(13);
            return;
        }
        //ビルの奥にあっても終了
        if (builhit.distance < weekpoints[0].distance)
        {
            if (XrayMachines.xrayMachineObjects.Count <= 1) gameManager.XrayZero();
            GameTextController.FailedText(13);
            return;
        }

        int minusPoint = 0;

        if (builhit.distance < MainStageDate.TroutLengthX * 5) { } //5マス以内だったら何もしない
        else if (builhit.distance < MainStageDate.TroutLengthX * 6) { minusPoint = 2; }
        else if (builhit.distance < MainStageDate.TroutLengthX * 7) { minusPoint = 3; }
        else { minusPoint = 5; }

        //離れすぎの場合、写ってる弱点の数を減らす。0個以下になったら終了
        if (weekpoints.Length - minusPoint <= 0)
        {
            if (XrayMachines.xrayMachineObjects.Count <= 1) gameManager.XrayZero();
            GameTextController.FailedText(14);
            return;
        }

        List<GameObject> hitWeak = new List<GameObject>();

        //List<int> weeknums = new List<int>();
        for (int i = 0; i < weekpoints.Length; i++)
        {
            if (weekpoints.Length - minusPoint <= i)
            {
                //weekpoints[i].transform.GetComponent<WeekPoint>().HideObject();
                continue;
            }

            RaycastHit weekhit;
            //向いている方向によってレイのスタート位置を決める
            Vector3 raystartpos = (Mathf.Abs(m_XrayCameraObj.transform.forward.x) > Mathf.Abs(m_XrayCameraObj.transform.forward.z)) ?
                new Vector3(m_XrayCameraObj.transform.position.x, weekpoints[i].transform.position.y, weekpoints[i].transform.position.z) :
                new Vector3(weekpoints[i].transform.position.x, weekpoints[i].transform.position.y, m_XrayCameraObj.transform.position.z);
            Ray ray = new Ray(raystartpos, weekpoints[i].transform.position - raystartpos);
            //スタート位置からレイを飛ばして弱点に当たるかどうか
            if (Physics.Raycast(ray, out weekhit, m_XrayCamera.farClipPlane, weekLayerMask))
            {
                if (weekhit.transform.position == weekpoints[i].transform.position) //対象の弱点に当たった場合
                {
                    //weeknums.Add(weekpoints[i].transform.GetComponent<WeekPoint>().GetWeekNumber);
                    //weekPoints.Add(weekpoints[i].transform.gameObject);
                    hitWeak.Add(weekpoints[i].transform.gameObject);
                }
                else  //対象の弱点に当たらない場合
                {
                    //weekpoints[i].transform.GetComponent<WeekPoint>().HideObject();
                }
            }
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 3.0f);
        }

        ////骨複製、骨がアニメーションする場合はどうしようかな
        //GameObject boneObj = weekpoints[0].transform.parent.parent.gameObject;//.root.Find("WeekPoints").gameObject;
        //GameObject bone = Instantiate(boneObj);
        //bone.transform.parent = boneObj.transform.parent;
        //bone.transform.position = boneObj.transform.position;
        //boneObj.transform.position -= Vector3.up * underPos;
        //boneObj.transform.tag = "Untagged";
        //boneObj.transform.parent = transform.parent;
        //foreach (Transform child in weekpoints[0].transform.parent)
        //{
        //    //if (child.tag == "WeekPoint" && child.Find("model").gameObject.activeSelf) weekPoints.Add(child.gameObject);
        //    child.tag = "Untagged";
        //}

        //骨の元取得
        GameObject boneParent = weekpoints[0].transform.root.Find("WeekPoints").gameObject;
        //骨の複製
        GameObject cloneBone = Instantiate(boneParent, boneParent.transform.position, boneParent.transform.rotation);
        cloneBone.transform.position -= Vector3.up * underPos;
        cloneBone.transform.parent = transform.parent;
        //複製した骨のアニメを停止
        cloneBone.transform.Find("Body/robot").gameObject.GetComponent<Animator>().enabled = false;

        //複製した骨の全子供取得
        List<GameObject> boneChilds = GetAllChildren.GetAll(cloneBone);
        foreach (GameObject child in boneChilds)
        {
            //弱点じゃなかったら終わり
            if (child.tag != "WeekPoint") continue;

            //弱点のタグを変更し、ナンバー取得
            child.tag = "Untagged";
            int cnum = child.GetComponent<WeekPoint>().GetWeekNumber;

            //カメラからのレイが当たった弱点と同じ弱点を探す
            foreach (GameObject hitPoint in hitWeak)
            {
                if (hitPoint.GetComponent<WeekPoint>().GetWeekNumber == cnum)
                {
                    weekPoints.Add(child);
                }
            }
        }


        //weeknums.Sort();
        //if (gameManager != null) gameManager.SetWeekPhoto(texnumber, weeknums);


        //レイが当たったビルの幕に撮ったやつを出す
        builhit.transform.parent.Find("XrayMaku").gameObject.SetActive(true);
    }

    private void SendForManager(List<int> weeknums)
    {
        weeknums.Sort();
        if (gameManager != null)
        {
            gameManager.SetWeekPhoto(texnumber, weeknums);
            if (XrayMachines.xrayMachineObjects.Count <= 1) gameManager.XrayZero();
        }
    }


    /// <summary>撮影（プレイヤーが呼ぶ関数）</summary>
    public void XrayPlay()
    {
        if (xrayOK)
        {
            Shooting();
            m_XrayCameraObj.transform.position -= Vector3.up * underPos;
            m_XrayCameraObj.transform.parent = transform.parent;

            GameObject flash = Instantiate(flashPrefab);
            flash.transform.parent = transform;
            flash.transform.localPosition = new Vector3(0, 5, 0);
            flash.transform.eulerAngles = transform.eulerAngles;
            m_checkIcon.SetActive(true);
            GetComponent<AudioSource>().Play();
            minimapIcon.enabled = false;
            minimapArrow.enabled = false;
            transform.tag = "XlineEnd";
            xrayOK = false;
            transform.Find("MapIcon").gameObject.SetActive(false);

            // LTボタンを押していない間
            if (!Xray_SSS.IsShutterChance)
            {
                XrayMachines.RemoveObj(gameObject);
            }
        }
    }

    /// <summary> 射影機移動して撮影して終わるのはLTを離したときにする </summary>
    public void XrayPlaySupport()
    {
        //自身の tag が変わっていて、まだ忘れられていないなら
        if (transform.CompareTag("XlineEnd") && XrayMachines.xrayMachineObjects.Contains(gameObject))
        {
            XrayMachines.RemoveObj(gameObject);
        }
    }

    /// <summary>csvからもらうデータ</summary>
    public void SetCSVData(string num)
    {
        texnumber = num;

        underPos *= int.Parse(texnumber.Substring(1, 1)); //別のカメラに映らないように下げる位置変更
    }

    private void DeadProcessing(string tagName)
    {
        if (tagName == "BigEnemy" || tagName == "Missile")
        {
            if (xrayOK) //撮影してない場合
            {
            }
            else if (saveTime > 0) //撮影している場合
            {
                int minus = (int)saveTime / 2;
                if (weekPoints.Count - minus > 0)
                {
                    List<int> weeknums = new List<int>();
                    for (int i = 0; i < weekPoints.Count; i++)
                    {
                        if (weekPoints.Count - minus <= i)
                        {
                            //weekPoints[i].transform.GetComponent<WeekPoint>().HideObject();
                            continue;
                        }
                        weekPoints[i].GetComponent<WeekPoint>().ActiveModel();
                        weeknums.Add(weekPoints[i].GetComponent<WeekPoint>().GetWeekNumber);
                    }
                    SendForManager(weeknums);
                }
                //m_XrayCameraObj.SetActive(false); //使ったカメラは非表示に
            }

            minimapIcon.enabled = false;
            minimapArrow.enabled = false;
            XrayMachines.RemoveObj(gameObject);
            Destroy(gameObject);

        }
    }

    public bool GetXrayOK()
    {
        return xrayOK;
    }

    private void OnCollisionEnter(Collision collision)
    {
        DeadProcessing(collision.transform.tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        DeadProcessing(other.transform.tag);
    }



}
