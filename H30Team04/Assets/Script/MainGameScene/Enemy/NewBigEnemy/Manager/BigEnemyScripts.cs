using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBigEnemy
{
    public class BigEnemyScripts : MonoBehaviour
    {
        public static Transform mTransform { get; private set; }
        public static BigEnemyMove bigEnemyMove { get; private set; }
        public static SearchObject searchObject { get; private set; }
        public static BigEnemyEffect bigEnemyEffect { get; private set; }
        public static BigEnemyAnimatorManager bigEnemyAnimatorManager { get; private set; }

        void Awake()
        {
            mTransform = transform;
            bigEnemyMove = GetComponentInChildren<BigEnemyMove>();
            searchObject = GetComponentInChildren<SearchObject>();
            bigEnemyEffect = GetComponentInChildren<BigEnemyEffect>();
            bigEnemyAnimatorManager = GetComponentInChildren<BigEnemyAnimatorManager>();
        }
    }
}