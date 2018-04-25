using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPhotos : MonoBehaviour {
   // Material m_BackGroundMaterial;
    bool m_IsSetTarget;
    float m_Value;
    float m_TargetPositionX;
    float m_MoveVelocity;
    // Use this for initialization
    void Start()
    {
       // m_BackGroundMaterial = GameObject.Find("SelectBackground").GetComponent<Image>().material;
        m_IsSetTarget = false;
        m_Value = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsSetTarget) return;
        float X = GetComponent<RectTransform>().localPosition.x;
        GetComponent<RectTransform>().localPosition = Vector3.right * Mathf.Lerp(GetComponent<RectTransform>().localPosition.x, m_TargetPositionX, m_Value);
        m_MoveVelocity = GetComponent<RectTransform>().localPosition.x - X;
      //  float l_t = m_BackGroundMaterial.GetFloat("_T");
      //  m_BackGroundMaterial.SetFloat("_T", l_t + (-m_MoveVelocity / 1280.0f));
        if (m_Value >= 1.0f)
        {
            m_IsSetTarget = false;
            m_Value = 1.0f;
            return;
        }
        m_Value += Time.deltaTime;
    }

    public bool IsSetTarget()
    {
        return m_IsSetTarget;
    }
    public bool IsReachTargetPositionX()
    {
        return m_Value >= 1.0f;
    }

    public void MoveTargetPositionX(float targetPositionX)
    {
        m_TargetPositionX = targetPositionX;
        m_IsSetTarget = true;
        m_Value = 0.0f;
    }
}
