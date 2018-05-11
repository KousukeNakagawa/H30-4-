using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLight : MonoBehaviour
{
    [SerializeField] MeshRenderer m_light;
    Color m_lightColor;

    void Start()
    {
        m_lightColor= m_light.material.color;
    }

    void Update()
    {
        m_lightColor.a = 0;
        //m_light.material.color = Color.black;
    }
}
