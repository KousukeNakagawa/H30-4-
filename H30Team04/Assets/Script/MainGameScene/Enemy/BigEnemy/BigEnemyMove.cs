using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyMove : MonoBehaviour
{
    public float xSpeed = 8.0f;
    public float turnSpeed = 40.0f;

    private Vector3 turnDir;
    private Vector3 turnEndDir;
    private bool isTurn;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (BigEnemyScripts.searchObject.isSearch)
        {
            float dir = GetDirction(transform.position, BigEnemyScripts.searchObject.targetPos);
            Vector3 endDir = BigEnemyScripts.mTransform.localEulerAngles;
            endDir.y = Mathf.Rad2Deg * dir;
            SetTurn(endDir);
            BigEnemyScripts.searchObject.isSearch = false;
        }

        if (isTurn)
        {
            turnDir.y += BigEnemyScripts.searchObject.turnVel * turnSpeed * Time.deltaTime;
            turnDir = turnDir.GetUnityVector3();
            BigEnemyScripts.mTransform.rotation = Quaternion.Euler(turnDir);
            if (Mathf.Abs(turnEndDir.y - turnDir.y) <= turnSpeed * Time.deltaTime)
            {
                isTurn = false;
                if (BigEnemyScripts.missileLaunch.isMissile) BigEnemyScripts.missileLaunch.LaunchSet();
            }
        }
        else
        {
            if (!BigEnemyScripts.missileLaunch.isMissile)
            {
                BigEnemyScripts.mTransform.Translate(xSpeed * Time.deltaTime, 0, 0, Space.Self);
            }
        }
    }

    private float GetDirction(Vector3 self, Vector3 target)
    {
        Vector2 dirVec2 = new Vector2(target.x - self.x, target.z - self.z).normalized;
        float dir = Mathf.Atan2(-dirVec2.y, dirVec2.x);
        return dir;
    }

    public void SetTurn(Vector3 endDir)
    {
        turnEndDir = endDir.GetUnityVector3();
        turnDir = BigEnemyScripts.mTransform.localEulerAngles.GetUnityVector3();
        isTurn = true;
    }

    public void SetGoDefenseLine()
    {
        BigEnemyScripts.searchObject.SetTurnVelGoDefenseLine();
        isTurn = true;
        turnDir = BigEnemyScripts.mTransform.localEulerAngles.GetUnityVector3();
        turnEndDir = Vector3.zero;
    }
}
