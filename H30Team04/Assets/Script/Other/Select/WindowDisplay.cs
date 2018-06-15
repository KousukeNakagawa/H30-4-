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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
