using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchObject : MonoBehaviour
{

    [HideInInspector] public float turnVel;
    public GameObject searchTarget;
    [HideInInspector] public bool isSearch;
    public Vector3 targetPos;
    private float dontSearchTime;

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
        if ((other.CompareTag("Player") || other.CompareTag("Xline") || other.CompareTag("Beacon")))
        {
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
    {
        Vector3 transDot = BigEnemyScripts.mTransform.TransformDirection(Vector3.forward);
        float dot = Vector2.Dot(new Vector2(transDot.x, transDot.z),
            new Vector2(target.transform.position.x - BigEnemyScripts.mTransform.position.x,
            target.transform.position.z - BigEnemyScripts.mTransform.position.z).normalized);
        turnVel = dot / Mathf.Abs(dot) * -1;
    }

    public void SetTurnVelGoDefenseLine()
    {
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
        while(dontSearchTime > 0)
        {
            dontSearchTime -= Time.deltaTime;
            yield return null;
        }
    }
}
