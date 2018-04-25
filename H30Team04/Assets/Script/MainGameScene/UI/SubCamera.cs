using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCamera : MonoBehaviour {
    GameObject m_mainCamera;
    
	// Use this for initialization
	void Start () {
        m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = m_mainCamera.transform.position;
        transform.rotation = m_mainCamera.transform.rotation;
	}
}
