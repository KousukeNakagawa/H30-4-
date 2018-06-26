using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialXLineMarker : MonoBehaviour {

    GameObject m_player;
    TutorialXray_SSS m_xsss;
    int XlineCount;
    bool test = false;
    [SerializeField]
    GameObject m_mainCamera;

    // Use this for initialization
    void Start()
    {
        //  m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_xsss = m_player.GetComponent<TutorialXray_SSS>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_mainCamera.transform.position);

        if (m_xsss.GetTarget() == null) return;

        transform.position = new Vector3(m_xsss.GetTarget().transform.position.x, m_xsss.GetTarget().transform.position.y + 5, m_xsss.GetTarget().transform.position.z);
    }
}
