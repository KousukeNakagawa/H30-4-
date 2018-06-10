using UnityEngine;

namespace NewBigEnemy
{
    public class BigEnemyMove : UpdateRoot
    {
        public enum MoveType
        {
            Normal,  //通常（防衛ラインへ）
            Search,  //追尾状態
        }

        public Vector3 velocity { get; private set; }
        public float moveSpeed = 4.0f;
        public float turnSpeed = 40.0f;

        public MoveType movetype { get; private set; }
        private Vector3 turnEndDir;
        private Vector3 turnDir;

        void Awake()
        {
            movetype = MoveType.Normal;
            velocity = new Vector3(1.0f, 0.0f, 0.0f).normalized;
        }
        public override void UpdateMe()
        {
            switch (movetype)
            {
                case MoveType.Normal:
                    NormalMove();
                    break;
                case MoveType.Search:
                    SearchTurn();
                    break;
            }
        }

        private void NormalMove()
        {
            float vel = BigEnemyScripts.bigEnemyAnimatorManager.moveSpeed;
            BigEnemyScripts.mTransform.Translate(velocity * Time.deltaTime * moveSpeed * vel, Space.Self);
        }

        private void SearchTurn()
        {
            if (BigEnemyScripts.searchObject.isSearch)
            {
                SetTurn(GetTurnAngle(BigEnemyScripts.searchObject.targetPos));
                BigEnemyScripts.searchObject.isSearch = false;
            }
            if (!(Mathf.Abs(turnEndDir.y - turnDir.y) <= turnSpeed * Time.deltaTime))
            {
                float speed = BigEnemyScripts.bigEnemyAnimatorManager.moveSpeed;
                turnDir.y += BigEnemyScripts.searchObject.turnVel * turnSpeed * speed * Time.deltaTime;
                turnDir = turnDir.GetUnityVector3();
                BigEnemyScripts.mTransform.rotation = Quaternion.Euler(turnDir);
            }
            else
            {
                ChangeType(MoveType.Normal);
            }

        }

        public void ChangeType(MoveType type)
        {
            movetype = type;
        }

        public Vector3 GetTurnAngle(Vector3 target)
        {
            float dir = GetDirection(BigEnemyScripts.mTransform.position, target);
            Vector3 endDir = BigEnemyScripts.mTransform.eulerAngles.GetUnityVector3();
            endDir.y = Mathf.Rad2Deg * dir;
            return endDir;
        }

        private float GetDirection(Vector3 own,Vector3 target)
        {
            Vector2 dir = target.ToTopView() - own.ToTopView();
            float d = Mathf.Atan2(-dir.y, dir.x);
            return d;
        }

        public void SetTurn(Vector3 endDir)
        {
            turnEndDir = endDir.GetUnityVector3();
            turnDir = BigEnemyScripts.mTransform.eulerAngles.GetUnityVector3();
        }

        public void SetGoDefenseLine()
        {
            BigEnemyScripts.searchObject.SetTurnGoDefenseLine();
            ChangeType(MoveType.Search);
            turnDir = BigEnemyScripts.mTransform.eulerAngles.GetUnityVector3();
            turnEndDir = Vector3.zero;
        }
    }
}
