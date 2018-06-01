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

    [SerializeField] private GameObject roketPrefab;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Vector3 fireRightPos;
    [SerializeField] private Text m_week_name;
    private string m_weekText;

    private bool is_shot = false;


    // Use this for initialization
    void Start () {

    }

    private void Awake()
    {
        weekPoints = GameObject.FindGameObjectsWithTag("WeekPoint");
        target = GameObject.Instantiate(targetPrefab).transform;
        target.position = weekPoints[selectNum].transform.position;
        m_weekText = weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekName();
        transform.LookAt(target);
        m_gm = m_gamemanager.GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update () {

        if (m_gm.AttackStateNow() && !is_shot)
        {
            if (Input.GetButtonDown("Select"))
            {
                selectNum++;
                if (selectNum >= weekPoints.Length) selectNum = 0;
                m_weekText = weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekName();
                //m_week_name.text = m_weekText;
            }
            target.position = Vector3.Lerp(target.position, weekPoints[selectNum].transform.position, 0.5f);
            transform.LookAt(target);

            if (Input.GetButtonDown("Shutter"))
            {
                is_shot = true;
                GameObject roket = Instantiate(roketPrefab,transform);
                roket.transform.localPosition = fireRightPos;
                roket.transform.LookAt(target);
                BigEnemyScripts.shootingPhaseMove.enabled = false;
                m_gm.ChengeWait();
                //weekPoints[0].transform.root.GetComponent<BigEnemyScripts>().
                //m_gm.Damege(weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekNumber);
            }
        }
    }

    public string WeekName
    {
        get { return m_weekText; }
        set { m_weekText = value; }
    }

    public void Damege()
    {
        m_gm.Damege(weekPoints[selectNum].GetComponent<WeekPoint>().GetWeekNumber);
    }

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnTriggerEnter(Collider other)
    {

        Fade.ColorChenge(Color.white);
        Fade.FadeOut();
    }




}
