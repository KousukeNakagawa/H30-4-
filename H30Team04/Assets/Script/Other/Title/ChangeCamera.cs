using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class ChangeCamera : MonoBehaviour
{

    public GameObject mainCamera;
    public GameObject sabuCamera1;
    public GameObject sabuCamera2;
    public GameObject sabuCamera3;

    private float moveTime = 0f;
    public float changeTime1 = 3.0f;
    public float changeTime2 = 6.0f;
    public float changeTime3 = 9.0f;
    public float changeTime4 = 12.0f;

    private bool a = true;
    private bool b = true;
    private bool c = true;
    private bool d = true;

    private List<GameObject> myList = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        moveTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        moveTime += Time.deltaTime;
        if (moveTime >= changeTime1 && a)
        {
            mainCamera.SetActive(!mainCamera.activeSelf);
            sabuCamera1.SetActive(!sabuCamera1.activeSelf);
            a = false;
            b = true;
        }
        else if (moveTime >= changeTime2 && b)
        {
            sabuCamera1.SetActive(!sabuCamera1.activeSelf);
            sabuCamera2.SetActive(!sabuCamera2.activeSelf);
            b = false;
            c = true;
        }
        else if (moveTime >= changeTime3 && c)
        {
            sabuCamera2.SetActive(!sabuCamera2.activeSelf);
            sabuCamera3.SetActive(!sabuCamera3.activeSelf);
            c = false;
            d = true;
        }
        else if (moveTime >= changeTime4 && !b)
        {
            sabuCamera3.SetActive(!sabuCamera3.activeSelf);
            mainCamera.SetActive(!mainCamera.activeSelf);
            d = false;
            a = true;
            moveTime = 0;
        }
    }
}
