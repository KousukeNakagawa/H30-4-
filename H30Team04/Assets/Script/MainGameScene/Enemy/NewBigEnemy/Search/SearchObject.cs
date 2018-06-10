using System.Collections.Generic;
using UnityEngine;

namespace NewBigEnemy
{
    public class SearchObject : UpdateRoot
    {
        [HideInInspector] public float turnVel;  //-1で半時計回り、1で時計周り
        [HideInInspector] public GameObject searchTarget;  //突進、ミサイルの標的
        [HideInInspector] public bool isSearch;  //探索範囲に入ったか
        [HideInInspector] public Vector3 targetPos;  //突進、ミサイルの目標座標
        private float dontSearchTime;  //多重に判定させないため

        //優先順位
        private Dictionary<string, int> priority = new Dictionary<string, int>
        {
            {"Player",1 },
            {"Beacon",2 },
            {"XlineEnd",3 },
            {"Xline", 4 },
        };
        //射影機を探知出来る距離
        public float searchRange = 10.0f;
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, searchRange);
        }

        public override void UpdateMe()
        {
            if (dontSearchTime > 0)
            {
                dontSearchTime = Mathf.Clamp(dontSearchTime - Time.deltaTime, 0, float.MaxValue);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (dontSearchTime > 0) return;
            if (other.CompareTag("Player") || other.CompareTag("Beacon") ||
                (other.tag.Contains("Xline") && Vector2.Distance(other.transform.position.ToTopView(),
                BigEnemyScripts.mTransform.position.ToTopView()) < searchRange))
            {
                SetTarget(other.gameObject);
            }
        }

        public void SetTarget(GameObject target)
        {
            if (searchTarget == null || priority[target.tag] < priority[searchTarget.tag])
            {
                searchTarget = target;
                targetPos = target.transform.position;
                isSearch = true;
                SetTurn(target);
                BigEnemyScripts.bigEnemyMove.ChangeType(BigEnemyMove.MoveType.Search);
            }
        }

        private void SetTurn(GameObject target)
        {
            Vector3 transDot = BigEnemyScripts.mTransform.TransformDirection(Vector3.forward);
            float dot = Vector2.Dot(new Vector2(transDot.x, transDot.z),
                new Vector2(target.transform.position.x - BigEnemyScripts.mTransform.position.x,
                target.transform.position.z - BigEnemyScripts.mTransform.position.z).normalized);
            turnVel = -Mathf.Sign(dot);
        }

        public void SetTurnGoDefenseLine()
        {
            Vector3 eulerAngle = BigEnemyScripts.mTransform.eulerAngles.GetUnityVector3();
            float vel = eulerAngle.y;
            turnVel = -Mathf.Sign(vel);
        }

        public void ResetTarget()
        {
            isSearch = false;
            searchTarget = null;
            turnVel = 0;
            targetPos = Vector3.zero;
            dontSearchTime = 0.2f;
        }
    }
}
