using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhDController : MonoBehaviour {
    Animator m_animator;
    float m_speed = 0;

    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        m_animator.SetFloat("Speed", m_speed);
    }
}
