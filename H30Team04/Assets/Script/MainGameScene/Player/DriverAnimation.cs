using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverAnimation : MonoBehaviour
{
    Animator _anime;

    //List<bool> animes = new List<bool>();
    bool front = true;
    bool back = false;
    bool right = false;
    bool left = false;

    void Start()
    {
        _anime = GetComponent<Animator>();
        //animes.Add(front);
        //animes.Add(back);
        //animes.Add(right);
        //animes.Add(left);
    }

    void Update()
    {
        //運転操作の取得
        float axel = Input.GetAxis("Axel");
        float curve = /*Input.GetAxis("Curve")*/0;

        //アニメーション条件
        if (axel < 0)
        {
            front = false;
            back = true;
            right = false;
            left = false;
        }
        else if (curve > 0)
        {
            front = false;
            back = false;
            right = true;
            left = false;
        }
        else if (curve < 0)
        {
            front = false;
            back = false;
            right = false;
            left = true;
        }
        else if (axel >= 0)
        {
            front = true;
            back = false;
            right = false;
            left = false;
        }

        //アニメーションセット
        _anime.SetBool("Front", front);
        _anime.SetBool("Back", back);
        _anime.SetBool("Right", right);
        _anime.SetBool("Left", left);
    }

    /// <summary>
    /// 指定したアニメーションを開始、それ以外のアニメーションは終了
    /// </summary>
    /// <param name="animation"></param>
    //void ChangeAnimation(bool animation)
    //{
    //    animation = true;
    //    for (int i = 0; i < animes.Count; i++)
    //        if (animation != animes[i]) animes[i] = false;
    //}
}