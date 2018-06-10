using UnityEngine;

namespace NewBigEnemy
{
    public class DestinationArrive : UpdateRoot
    {
        [SerializeField] private float targetMagnitudeRange = 5.0f;

        public override void UpdateMe()
        {
            Vector3 targetPos = BigEnemyScripts.searchObject.targetPos;
            if (targetPos != Vector3.zero)
            {
                print(Vector2.Distance(targetPos.ToTopView(), BigEnemyScripts.mTransform.position.ToTopView()));
                //目的地に到着していたら突進を終了する
                if (Vector2.Distance(targetPos.ToTopView(), BigEnemyScripts.mTransform.position.ToTopView()) < targetMagnitudeRange)
                {
                    BigEnemyScripts.searchObject.ResetTarget();
                    BigEnemyScripts.bigEnemyMove.SetGoDefenseLine();
                }
            }
        }
    }
}
