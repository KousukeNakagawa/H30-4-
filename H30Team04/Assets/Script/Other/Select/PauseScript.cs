using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class PauseScript : MonoBehaviour
{

    //　ポーズした時に表示するUI
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject pauseUI2;


    // Update is called once per frame
    void Update()
    {
        //　ポーズUIが表示されてる時は停止
        if (pauseUI.activeSelf && pauseUI2.activeSelf)
        {
            Time.timeScale = 0f;
            //　ポーズUIが表示されてなければ通常通り進行
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
