using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchObject : MonoBehaviour
{

    [HideInInspector] public float turnVel;  //-1で半時計回り、1で時計周り
    [HideInInspector] public GameObject searchTarget;  //突進、ミサイルの標的
    [HideInInspector] public bool isSearch;  //探索範囲に入ったか
    [HideInInspector] public Vector3 targetPos;  //突進、ミサイルの目標座標
    private float dontSearchTime;  //多重に判定させないため

    //  この範囲内にプレイヤーまたは射影機が入っているかの数値　　1.5マス分離れたらにしたかったけど、高さあったからとりあえず２
    private float searchsqrMagnitude = (MainStageDate.TroutLengthX * 2.0f) * (MainStageDate.TroutLengthX * 2.0f);

    //優先順位
    private Dictionary<string, int> priority = new Dictionary<string, int>
    {
        {"Player",1 },
        {"Beacon",2 },
        {"Xline", 3 },
    };

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay(Collider other)
    {
        if (dontSearchTime > 0) return;
        if ((other.CompareTag("Player") && (other.transform.position - BigEnemyScripts.mTransform.position).sqrMagnitude < searchsqrMagnitude
            || other.CompareTag("Xline") && (other.transform.position - BigEnemyScripts.mTransform.position).sqrMagnitude < searchsqrMagnitude ||
            other.CompareTag("Beacon")))
        {
            //予備動作中ならミサイルのターゲットを変更するメソッドへ
            if (!BigEnemyScripts.missileLaunch.isMissile) SetTarget(other.gameObject);
            else MissileTargetChange(other.gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        if (searchTarget == null || priority[target.tag] < priority[searchTarget.tag])
        {
            searchTarget = target;
            SetTurnVel(target);
            targetPos = target.transform.position;
            isSearch = true;
            if (target.CompareTag("Xline")) BigEnemyScripts.missileLaunch.isMissile = true;
            else BigEnemyScripts.missileLaunch.isMissile = false;
        }
    }

    public void MissileTargetChange(GameObject target)
    {
        if (!BigEnemyScripts.missileLaunch.isLaunch) return;
        if (priority[target.tag] < priority[searchTarget.tag])
        {
            searchTarget = target;
            targetPos = target.transform.position;
        }
    }

    private void SetTurnVel(GameObject target)
    {   //Z軸を中心に、右にいる場合は1を、左にいる場合はｰ1をturnVelに代入する
        Vector3 transDot = BigEnemyScripts.mTransform.TransformDirection(Vector3.forward);
        float dot = Vector2.Dot(new Vector2(transDot.x, transDot.z),
            new Vector2(target.transform.position.x - BigEnemyScripts.mTransform.position.x,
            target.transform.position.z - BigEnemyScripts.mTransform.position.z).normalized);
        turnVel = dot / Mathf.Abs(dot) * -1;
    }

    public void SetTurnVelGoDefenseLine()
    {  //現在のY軸回転の符号を反転させたものをturnVelに代入する
        Vector3 eulerAngle = BigEnemyScripts.mTransform.localEulerAngles.GetUnityVector3();
        float vel = eulerAngle.y;
        turnVel = vel / Mathf.Abs(vel) * -1;
    }

    public void ResetTarget()
    {
        isSearch = false;
        searchTarget = null;
        turnVel = 0;
        targetPos = Vector3.zero;
        dontSearchTime = 0.2f;
        StartCoroutine("DontSeacrchCountDown");
    }

    IEnumerator DontSeacrchCountDown()
    {
        while (dontSearchTime > 0)
        {
            dontSearchTime -= Time.deltaTime;
            yield return null;
        }
    }
}
