using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPlayer : MonoBehaviour {

    public GameObject m_gamemanager;
    GameManager m_gm;

    private GameObject[] weekPoints;
    private Transform target;
    private int selectNum = 0;

    [SerializeField] private GameObject zoomCamera;
    [SerializeField] private float zoomXoffset = 5.0f;


    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Vector3 fireRightPos;
    [SerializeField, Range(0.1f, 1.0f), Tooltip("スティック移動のインターバル")] private float m_Interval = 0.5f;
    private float intervalTime = 1.0f;
    private string m_weekText;

    private bool is_shot = false;

    [SerializeField] private Transform overCamera;
    [SerializeField] private Transform clearCamera;

    private GameObject m_rocket;
    private GameObject m_Camera;

    // Use this for initialization
    void Start () {
        m_Camera = transform.Find("Camera").gameObject;
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update () {

        if(target == null)
        {
            SetData();
        }
        else if (m_gm.AttackStateNow() && !is_shot)
        {
            TargetChange();

            Vector3 zoomCamePos = weekPoints[selectNum].transform.position;
            zoomCamePos.x = BigEnemyScripts.mTransform.position.x + zoomXoffset;
            zoomCamera.transform.position = Vector3.Lerp(zoomCamera.transform.position, zoomCamePos, 0.5f);

            target.position = Vector3.Lerp(target.position, weekPoints[selectNum].transform.position, 0.5f);
            transform.LookAt(target);

            if (Input.GetAxis("newFire") < 0)
            {
                RocketShot();
            }
        }
        else if(m_rocket != null)
        {
            FollowRocket();
        }
    }

    private void TargetChange()
    {
        if (Input.GetButtonDown("Select") || Input.GetAxisRaw("Hor") > 0.7f && intervalTime > m_Interval)
        {
            selectNum++;
            if (selectNum >= weekPoints.Length) selectNum = 0;
            m_weekText = weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekName();
            intervalTime = 0.0f;
        }
        else if (Input.GetAxisRaw("Hor") < -0.7f && intervalTime > m_Interval)
        {
            selectNum--;
            if (selectNum < 0) selectNum = weekPoints.Length - 1;
            m_weekText = weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekName();
            intervalTime = 0.0f;
        }
        intervalTime += Time.deltaTime;
    }

    private void RocketShot()
    {
        is_shot = true;
        GameObject rocket = Instantiate(rocketPrefab, transform);
        m_rocket = rocket;

        transform.eulerAngles = clearCamera.eulerAngles;
        rocket.transform.localPosition = fireRightPos;
        rocket.transform.LookAt(target);
        //BigEnemyScripts.shootingPhaseMove.enabled = false;
        m_gm.ChengeWait();


        zoomCamera.SetActive(false);
    }

    private void FollowRocket()
    {
        if(m_Camera.transform.position.x > m_rocket.transform.position.x + zoomXoffset/2)
        {
            Vector3 newPos = m_Camera.transform.position;
            newPos.x = m_rocket.transform.position.x + zoomXoffset/2;
            m_Camera.transform.position = newPos;
        }
    }

    private void SetData()
    {
        weekPoints = GameObject.FindGameObjectsWithTag("WeekPoint");
        target = GameObject.Instantiate(targetPrefab).transform;
        target.position = weekPoints[selectNum].transform.position;
        m_weekText = weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekName();
        m_gm = m_gamemanager.GetComponent<GameManager>();

        zoomCamera.SetActive(true);
    }

    public string WeekName
    {
        get { return m_weekText; }
        set { m_weekText = value; }
    }

    public int WeekPar
    {
        get { return weekPoints[selectNum].GetComponent<WeekPoint>().Par; }
    }

    public void Damege()
    {
        m_gm.Damege(weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekNumber);
    }

    public void EndEffect(bool clear)
    {
        m_Camera.transform.localPosition = Vector3.zero;
        if (clear)
        {
            transform.position = clearCamera.position;
            transform.eulerAngles = clearCamera.eulerAngles;
        }
        else
        {
            transform.position = overCamera.position;
            transform.eulerAngles = overCamera.eulerAngles;
            zoomCamera.SetActive(false);
        }
    }

}
