using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieFade : MonoBehaviour
{
    FadeObject fade;
    [SerializeField] GameObject _image;

    // Use this for initialization
    void Start()
    {
        fade = _image.GetComponent<FadeObject>();
    }

    // Update is called once per frame
    void Update()
    {
        ////フェード処理
        //if (Soldier.IsDead) fade.FadeOut();
        //else fade.FadeIn();

        //if (Input.GetButtonDown("Select")) fade.IsFade = true;
    }
}
