using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AviationObstacleLights : MonoBehaviour {

    GameObject[] m_Lights;

    // Use this for initialization
    void Start() {
        m_Lights = GameObject.FindGameObjectsWithTag("AviationLight");
        StartCoroutine("Light");
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator Light()
    {
        while (true)
        {
            foreach (GameObject _lights in m_Lights)
            {
                _lights.GetComponent<MeshRenderer>().enabled = !_lights.GetComponent<MeshRenderer>().enabled;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
