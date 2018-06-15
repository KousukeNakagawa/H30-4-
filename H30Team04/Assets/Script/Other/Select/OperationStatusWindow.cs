using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class OperationStatusWindow : MonoBehaviour
{

    [SerializeField]
    private GameObject propertyWindow;
   

    void Update()
    {
        //　ステータスウインドウのオン・オフ
        if (Input.GetButtonDown("Restart"))
        {
            propertyWindow.SetActive(!propertyWindow.activeSelf);
            //　MainWindowをセット
            //ChangeWindow(windowLists[0]);
        }
        if (Input.GetButtonDown("Cancel"))
        {
            propertyWindow.SetActive(false);
        }
    }
}