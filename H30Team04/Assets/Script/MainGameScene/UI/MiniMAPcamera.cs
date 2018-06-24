using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMAPcamera : MonoBehaviour
{
    [SerializeField]
    GameObject m_Player;
    [SerializeField]
    float cameraH = 20;
    Camera m_camera;
    [SerializeField]
    RenderTexture m_minimapRender;
    [SerializeField]
    RenderTexture m_mapRender;

    [SerializeField]
    GameObject m_Minimap;
    [SerializeField]
    GameObject m_Map;

    Rect _rect = new Rect(0, 0, 1, 1);
    public bool _pose = false;

    public GameManager gm;

    // Use this for initialization
    void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.NowState() == GameManager.PhaseState.switchState)
        {
            m_Map.SetActive(false);
            m_Minimap.SetActive(false);
            this.enabled = false;
            return;
        }

        if (!_pose && Time.timeScale==1)
        {
            m_Map.SetActive(false);
            m_Minimap.SetActive(true);
            m_camera.targetTexture = m_minimapRender;
            m_camera.orthographicSize = 50;
            transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + cameraH, m_Player.transform.position.z);
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        else
        {
            m_Minimap.SetActive(false);
            m_Map.SetActive(true);
            m_camera.targetTexture = m_mapRender;
            m_camera.orthographicSize = 200;
            transform.position = new Vector3(191, 145, -111);
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
    }

    public Rect MiniCameraRect
    {
        get { return _rect; }
        set { _rect = value; }
    }

    public bool Pose
    {
        get { return _pose; }
        set { _pose = value; }
    }
}
