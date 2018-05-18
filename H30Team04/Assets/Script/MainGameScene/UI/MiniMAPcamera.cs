using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMAPcamera : MonoBehaviour {
    [SerializeField]
    GameObject m_Player;
    [SerializeField]
    float cameraH = 20;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + cameraH, m_Player.transform.position.z);
    }
}
