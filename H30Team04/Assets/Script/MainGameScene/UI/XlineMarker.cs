using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XlineMarker : MonoBehaviour
{
    GameObject m_player;
    Xray_SSS m_xsss;
    [SerializeField]
    GameObject m_Xliner; //射影機
    int XlineCount;
    bool test=false;
    GameObject m_mainCamera;

    // Use this for initialization
    void Start()
    {
        m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        gameObject.SetActive(false);
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_xsss = m_player.GetComponent<Xray_SSS>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_mainCamera.transform.position);

        transform.position = new Vector3( m_xsss.GetTargetXray().transform.position.x, m_xsss.GetTargetXray().transform.position.y + 3, m_xsss.GetTargetXray().transform.position.z);
    }

    public void Mark()
    {
        gameObject.SetActive(true);
    }
}