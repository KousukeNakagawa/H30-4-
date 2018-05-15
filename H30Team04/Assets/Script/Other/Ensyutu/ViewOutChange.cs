using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewOutChange : MonoBehaviour {

    [SerializeField] private string changeScene;
    [Header("どれだけ離れたらチェンジか"),SerializeField] private float distance = 50.0f;
    private Fade fade;

    private void Start()
    {
        fade = GameObject.Find("fade").GetComponent<Fade>();
    }

    private void Update()
    {
        if ((Camera.main.transform.position - transform.position).sqrMagnitude > distance * distance)
        {
            if (!fade.IsFadeOutOrIn())  fade.FadeOut();
            if(fade.IsFadeEnd())SceneManager.LoadScene(changeScene);
        }
    }

    //void OnBecameInvisible()
    //{
    //    SceneManager.LoadScene(changeScene);

    //}
}
