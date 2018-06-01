using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class OperationStatusWindow : MonoBehaviour
{

    [SerializeField]
    private GameObject propertyWindow;
    //　ステータスウインドウの全部の画面
    [SerializeField]
    private GameObject[] windowLists;
    [SerializeField]
    private GameObject firstSelect;
    [SerializeField]
    private GameObject TiteleReturnWindow;
    [SerializeField]
    private GameObject secondSelect;

    void Update()
    {
        //　ステータスウインドウのオン・オフ
        if (Input.GetButtonDown("BackAngle")|| Input.GetKeyDown(KeyCode.Space))
        {
            propertyWindow.SetActive(!propertyWindow.activeSelf);
            //　MainWindowをセット
            ChangeWindow(windowLists[0]);
        }
        if (Input.GetButtonDown("Cancel"))
        {
            propertyWindow.SetActive(false);
        }
    }

    //　ステータス画面のウインドウのオン・オフメソッド
    public void ChangeWindow(GameObject window)
    {
        foreach (var item in windowLists)
        {
            if (item == window)
            {
                item.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                item.SetActive(false);
            }
            //　それぞれのウインドウのMenuAreaの最初の子要素をアクティブな状態にする
            // EventSystem.current.SetSelectedGameObject(window.transform.Find("ManuArea").GetChild(0).gameObject);
            EventSystem.current.SetSelectedGameObject(firstSelect);
            if (TiteleReturnWindow.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(secondSelect);
            }
        }
    }
}