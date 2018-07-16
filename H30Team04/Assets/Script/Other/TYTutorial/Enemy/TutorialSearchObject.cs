using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSearchObject : MonoBehaviour {

    [HideInInspector] public float turnVel;  //-1で半時計回り、1で時計周り
    [HideInInspector] public GameObject searchTarget;  //突進、ミサイルの標的
    [HideInInspector] public bool isSearch;  //探索範囲に入ったか
    [HideInInspector] public Vector3 targetPos;  //突進、ミサイルの目標座標
    private float dontSearchTime;  //多重に判定させないため

    //  この範囲内にプレイヤーまたは射影機が入っているかの数値　　1.5マス分離れたらにしたかったけど、高さあったからとりあえず２
    //private float searchsqrMagnitude = (MainStageDate.TroutLengthX * 2.0f) * (MainStageDate.TroutLengthX * 2.0f);
    [Range(10, 45), Tooltip("射影機を探索する範囲（赤い円）"), SerializeField] private float searchXlineMagnitude = MainStageDate.TroutLengthX * 2;
    [Range(10, 45), Tooltip("プレイヤーを探索する範囲（青い円）"), SerializeField] private float searchPlayerMagnitude = MainStageDate.TroutLengthX * 2 * 1.25f;

    public GameObject aowaku;

    //優先順位
    private Dictionary<string, int> priority = new Dictionary<string, int>
    {
        {"Beacon",1 },
    };
    

    void OnTriggerStay(Collider other)
    {
        if (dontSearchTime > 0) return;
        if(other.CompareTag("Beacon"))
        {
            SetTarget(other.gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        if (searchTarget == null || priority[target.tag] < priority[searchTarget.tag])
        {
            if ((aowaku.transform.position.ToTopView() - target.transform.position.ToTopView()).sqrMagnitude >= 5 * 5)
            {
                return;
            }
            searchTarget = target;
            SetTurnVel(target);
            targetPos = target.transform.position;
            isSearch = true;
            TutorialEnemyScripts.bigEnemyMove.isDefense = false;
            TutorialEnemyScripts.tmane.RoboTurn = true;
        }
    }

    

    private void SetTurnVel(GameObject target)
    {   //Z軸を中心に、右にいる場合は1を、左にいる場合はｰ1をturnVelに代入する
        Vector3 transDot = TutorialEnemyScripts.mTransform.TransformDirection(Vector3.forward);
        float selfatan = Mathf.Atan2(TutorialEnemyScripts.mTransform.position.z, TutorialEnemyScripts.mTransform.position.x);
        float otheratan = Mathf.Atan2(target.transform.position.z, target.transform.position.x);
        float delta = Mathf.DeltaAngle(selfatan, otheratan);
        float dot = Vector2.Dot(new Vector2(transDot.x, transDot.z),
            new Vector2(target.transform.position.x - TutorialEnemyScripts.mTransform.position.x,
            target.transform.position.z - TutorialEnemyScripts.mTransform.position.z).normalized);
        turnVel = dot / Mathf.Abs(dot) * -1;
    }

    public void SetTurnVelGoDefenseLine()
    {  //現在のY軸回転の符号を反転させたものをturnVelに代入する
        Vector3 eulerAngle = TutorialEnemyScripts.mTransform.localEulerAngles.GetUnityVector3();
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
        StartCoroutine(DontSeacrchCountDown());
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
