using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowDisplay : MonoBehaviour  {
    [SerializeField]
    private GameObject Map;
    [SerializeField]
    private GameObject Control;
    [SerializeField]
    private GameObject Title;
    [SerializeField]
    private GameObject mapwindow;
    [SerializeField]
    private GameObject ControlWindow;
    [SerializeField]
    private GameObject TitleWindow;
    [SerializeField]
    private GameObject propertyWindow;
    [SerializeField]
    private MiniMAPcamera m_miniMapCamera;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //　ステータスウインドウのオン・オフ
        if (Input.GetButtonDown("Restart"))
        {
            propertyWindow.SetActive(!propertyWindow.activeSelf);
            Map.SetActive(false);
            m_miniMapCamera._pose = !m_miniMapCamera._pose;
        }
        if (Input.GetButtonDown("Cancel")&& m_miniMapCamera.Pose)
        {
            propertyWindow.SetActive(false);
            m_miniMapCamera._pose = !m_miniMapCamera._pose;
        }
        if (!Map.activeSelf)
        {
            mapwindow.SetActive(true);
            ControlWindow.SetActive(false);
            TitleWindow.SetActive(false);
        }
        else if (!Control.activeSelf)
        {
            ControlWindow.SetActive(true);
            mapwindow.SetActive(false);
            TitleWindow.SetActive(false);
        }
        else
        {
            TitleWindow.SetActive(true);
            ControlWindow.SetActive(false);
            mapwindow.SetActive(false);
        }
    }
}
