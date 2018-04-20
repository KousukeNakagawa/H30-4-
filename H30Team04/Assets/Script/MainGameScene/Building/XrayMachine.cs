using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XrayMachine : MonoBehaviour {

    private bool xrayOK = true;
    private GameObject m_XrayCameraObj;
    private Camera m_XrayCamera;

    private string texnumber = "01";
    private int weekLayerMask;
    private int builLayerMask;

    // Use this for initialization
    void Start () {
        m_XrayCameraObj = transform.Find("XrayCamera").gameObject;
        m_XrayCamera = m_XrayCameraObj.GetComponent<Camera>();
        //m_XrayCamera.targetTexture = Resources.Load("Texture/RenderTextures/XrayCamera" + texnumber) as RenderTexture;
        weekLayerMask = LayerMask.GetMask(new string[] { LayerMask.LayerToName(8) });
        builLayerMask = LayerMask.GetMask(new string[] { LayerMask.LayerToName(9) });
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Shooting();
        }
	}

    private void Shooting()
    {
        Debug.Log("食らいやがれー");

        //RaycastHit builhit = Physics.Ra

        RaycastHit[] weekpoints 
            = Physics.BoxCastAll(m_XrayCameraObj.transform.position,
            new Vector3(MainStageDate.TroutLengthX / 2, m_XrayCameraObj.transform.position.y, MainStageDate.TroutLengthZ / 2),
            m_XrayCameraObj.transform.forward, Quaternion.identity,m_XrayCamera.farClipPlane, weekLayerMask);
        for(int i = 0;i < weekpoints.Length; i++)
        {
            Debug.Log(weekpoints[i].transform.name);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(m_XrayCameraObj.transform.position, new Vector3(MainStageDate.TroutLengthX, m_XrayCameraObj.transform.position.y * 2, MainStageDate.TroutLengthZ));

    //}



    public void XrayPlay()
    {
        if (xrayOK)
        {
            //Shooting();
            xrayOK = false;
            transform.tag = "XlineEnd";
        }
    }

    public void SetTexNumber(string num)
    {
        texnumber = num;
    }
}
