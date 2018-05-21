using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;



//angleのスピードで、とあるオブジェクトの周りを回転するスクリプト

public class TargetRotation : MonoBehaviour
{



    //変数

    //一秒当たりの回転角度

    public float angle = 30f;



    //回転の中心をとるために使う変数

    private Vector3 targetPos;





    // Use this for initialization

    void Start()
    {

        //targetに、"Sample"の名前のオブジェクトのコンポーネントを見つけてアクセスする

        Transform target = GameObject.Find("CenterPoint").transform;

        //変数targetPosにSampleの位置情報を取得

        targetPos = target.position;



        //自分の向きをターゲットの正面に向ける

        //transform.LookAt(target);



        //自分をx軸を中心に0～360でランダムに回転させる

        transform.Rotate(new Vector3(360, 0, 0), Space.World);

    }



    void Update()
    {



        //	Sampleを中心に自分を現在の上方向に、毎秒angle分だけ回転する。

        Vector3 axis = transform.TransformDirection(Vector3.up);

        transform.RotateAround(targetPos, axis, angle * Time.deltaTime);

    }

}
