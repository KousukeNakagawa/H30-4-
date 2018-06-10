using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBigEnemy
{
    public class BigEnemyEffect : UpdateRoot
    {
        public enum Direction
        {
            Right,
            Left,
            None
        }
        [HideInInspector] public int runEffectNum;
        [HideInInspector] public Direction m_direction = Direction.None;
    }
}
