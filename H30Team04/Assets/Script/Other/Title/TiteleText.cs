using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiteleText : MonoBehaviour
{

    private GameObject textObj;

    private float nextTime;
    public float interval = 0.8f;//点滅時間

    // Use this for initialization
    void Start()
    {
        textObj = GameObject.Find("PushA");
        nextTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTime)
        {
            float alpha = textObj.GetComponent<CanvasRenderer>().GetAlpha();
            if (alpha == 1.0f)
            {
                textObj.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
            }
            else
            {
                textObj.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            }
            nextTime += interval;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Restart"))
        {
            Destroy(textObj);
        }
    }
}
