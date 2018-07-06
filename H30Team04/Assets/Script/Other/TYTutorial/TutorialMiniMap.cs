using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMiniMap : MonoBehaviour {

    [SerializeField]
    GameObject m_Player;
    [SerializeField]
    float cameraH = 20;
    Camera m_camera;
    [SerializeField]
    RenderTexture m_minimapRender;

    [SerializeField]
    GameObject m_Minimap;

    Rect _rect = new Rect(0, 0, 1, 1);

    public TutorialManager_T tmane;

    private float nextTime;
    private float interval = 0.8f;

    // Use this for initialization
    void Start()
    {
        m_camera = GetComponent<Camera>();
        //nextTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(tmane.GetTextNum() == 2)
        {
            if(nextTime == 0)
            {
                nextTime = Time.time;
                return;
            }
            if (Time.time > nextTime)
            {
                m_Minimap.SetActive(!m_Minimap.activeSelf);
                nextTime += interval;
            }
            return;
        }

        if (tmane.GetState() >= TutorialState_T.SHOTEFFECT)
        {
            m_Minimap.SetActive(false);
            this.enabled = false;
            return;
        }
        
            m_Minimap.SetActive(true);
            m_camera.targetTexture = m_minimapRender;
            m_camera.orthographicSize = 50;
            transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + cameraH, m_Player.transform.position.z);
           transform.rotation = Quaternion.Euler(90, 0, 0);
        
    }

    public Rect MiniCameraRect
    {
        get { return _rect; }
        set { _rect = value; }
    }
    
}
