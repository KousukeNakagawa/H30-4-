using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAttackPlayer : MonoBehaviour {

    public TutorialManager_T tmane;

    private GameObject[] weekPoints;
    private Transform target;
    private int selectNum = 0;
    
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Vector3 fireRightPos;
    [SerializeField, Range(0.1f, 1.0f), Tooltip("スティック移動のインターバル")] private float m_Interval = 0.5f;
    private float intervalTime = 1.0f;
    private string m_weekText;
    
    public GameObject roket;


    // Use this for initialization
    void Start()
    {
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (target == null)
        {
            SetData();
        }
        else if (tmane.GetState() == TutorialState_T.SHOT && !tmane.Shot)
        {
            if (!tmane.IsReaded()) return;
            if (Input.GetButtonDown("Select") || Input.GetAxisRaw("Hor") > 0.7f && intervalTime > m_Interval)
            {
                selectNum++;
                if (selectNum >= weekPoints.Length) selectNum = 0;
                m_weekText = weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekName();
                //m_week_name.text = m_weekText;
                intervalTime = 0.0f;
            }
            else if (Input.GetAxisRaw("Hor") < -0.7f && intervalTime > m_Interval)
            {
                selectNum--;
                if (selectNum < 0) selectNum = weekPoints.Length - 1;
                m_weekText = weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekName();
                //m_week_name.text = m_weekText;
                intervalTime = 0.0f;
            }
            intervalTime += Time.deltaTime;
            target.position = Vector3.Lerp(target.position, weekPoints[selectNum].transform.position, 0.5f);
            transform.LookAt(target);

            if (Input.GetButtonDown("Fire"))
            {
                //GameObject roket = Instantiate(roketPrefab, transform);
                roket.SetActive(true);
                roket.transform.localPosition = fireRightPos;
                roket.transform.LookAt(target);
                TutorialEnemyScripts.shootingPhaseMove.enabled = false;
                tmane.Shot = true;
                
            }
        }
    }

    private void SetData()
    {
        weekPoints = GameObject.FindGameObjectsWithTag("WeekPoint");
        target = GameObject.Instantiate(targetPrefab).transform;
        target.position = weekPoints[selectNum].transform.position;
        m_weekText = weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekName();
        //transform.LookAt(target);
    }

    public string WeekName
    {
        get { return m_weekText; }
        set { m_weekText = value; }
    }

    public int WeekPar
    {
        get { return weekPoints[selectNum].GetComponent<WeekPoint>().Par; }
        //set { m_weekText = value; }
    }

    public void Damege()
    {
        tmane.Damege(weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekNumber);
    }

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Missile")
        {
            Fade.ColorChenge(Color.white);
            Fade.FadeOut();
        }

    }



}
