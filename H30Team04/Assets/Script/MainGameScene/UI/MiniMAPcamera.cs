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
    Rect _rect = new Rect(0, 0, 1, 1);

    // Use this for initialization
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + cameraH, m_Player.transform.position.z);
        transform.rotation = Quaternion.Euler(90, 0, 0);

    }

    public Rect MiniCameraRect
    {
        get { return _rect; }
        set { _rect = value; }
    }
}
