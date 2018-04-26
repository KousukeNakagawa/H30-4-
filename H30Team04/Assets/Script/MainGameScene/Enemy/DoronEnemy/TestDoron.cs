using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoron : MonoBehaviour
{

    public float moveSpeed = 10f;        // 移動速度
    // 移動種類ごとの継続時間
    public float forwardtime = 1f;   //縦時間   
    public float sidetime = 7f;　　　//横時間
    public float flySpeed = 5.0f;　　//上昇スピード
    public float flyDeleteTime = 5.0f;　　    //消える時間
    public Direction direction = Direction.Advance;  //方向
    public float range = 65.0f;       //otherEnemyとのX座標の幅の限界
    public float rotMax = 1;　　　　　//回転　　　　
    public float backSpeed = 1;　　　 //BigEnemyに戻る速度
    //public GameObject otherDoron;
    public enum Direction
    {
        Recession,  //後退
        Advance = 2,  //前進
    }

    private bool m_yPlus = true;                // Y軸プラス方向に移動中か？
    private bool m_xPlus = false;
    private bool m_yPlus2 = false;
    private bool isH = false;
    private bool isFirstLeft = false;
    private Transform EnemyTransform;
    private Vector3 primaryPos;



    // 移動種類
    private enum MoveType
    {
        FORWARD_1,  // 前進１
        LEFT,       // 左へ
        FORWARD_2,  // 前進2
        RIGHT,       // 右へ
        END,  //終了
    }
    private MoveType moveType;
    private float moveTime = 0f;

    // Use this for initialization
    void Start()
    {
        moveTime = 0;
        EnemyTransform = BigEnemyScripts.mTransform;　//BigEnemyの位置
        transform.parent = null;
        primaryPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance2 = EnemyTransform.position - transform.position;　//BigEnemyとの距離
        var distance3 = (EnemyTransform.position.x - transform.position.x);　//BigEnemyとのx座標のみの距離
        var distance4 = (primaryPos.x - transform.position.x);  //otherEnemyとの距離

        if (m_yPlus)
        {
            transform.Translate(new Vector3(0.0f, flySpeed, moveSpeed) * Time.deltaTime);
        }
        else
        {
            switch (moveType)
            {
                case MoveType.FORWARD_1:
                case MoveType.FORWARD_2:
                    transform.Translate(transform.right * ((int)direction - 1) * moveSpeed * Time.deltaTime);
                    break;
                case MoveType.LEFT:
                    transform.Translate(-transform.forward * moveSpeed * Time.deltaTime);
                    break;
                case MoveType.RIGHT:
                    transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
                    break;
                case MoveType.END:
                    break;
            }
        }

        // 時間をカウントし、移動種類を切り替える
        moveTime += Time.deltaTime;

        float Tmax = (isH) ? forwardtime : sidetime;
        if (m_yPlus) Tmax = Tmax / 2;
        {
            if (moveTime > Tmax)
            {
                if (m_yPlus) m_yPlus = false;
                else
                {
                    if (moveType != MoveType.END) moveType = (MoveType)(((int)moveType + 1) % 4);
                    if (Mathf.Abs(distance4) >= range && moveType == MoveType.FORWARD_2)
                    {
                        moveType = MoveType.END;
                    }
                }
                isH = !isH;
                moveTime = 0;
            }
        }
        if (moveType == MoveType.END && !m_yPlus2)
        {
            //ターゲットの方向を向く
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(distance2.x, 0, distance2.z)), rotMax);
            //正面方向に移動
            transform.Translate(Vector3.forward * backSpeed);
            if (Mathf.Abs(distance3) <= 1)
            {
                m_yPlus2 = true;
            }
        }
        if (m_yPlus2)
        {
            if (moveTime < flyDeleteTime)
            {
                transform.Translate(new Vector3(0, -flySpeed) * Time.deltaTime);
                transform.parent = BigEnemyScripts.mTransform;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnDestroy()
    {
        BigEnemyScripts.droneCreate.RemoveDrone(gameObject);
    }
}
