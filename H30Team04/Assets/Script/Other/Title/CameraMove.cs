using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    private bool m_xPlus = true;

    public float xLine = 4.0f;
    public float RevxLine = 4.0f;
    public float xSpeed = 2.0f;
    public float ySpeed = 0.0f;
    public float zSpeed = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_xPlus)
        {
            transform.position += new Vector3((Time.deltaTime) * xSpeed, ySpeed, zSpeed);
            if (transform.position.x >= xLine)
                m_xPlus = false;
        }
        else
        {
            transform.position -= new Vector3((Time.deltaTime) * xSpeed, ySpeed, zSpeed);
            if (transform.position.x <= RevxLine)
                m_xPlus = true;

        }
    }
}
