using UnityEngine;

public enum Direction
{
    Right,
    Left,
    None,
}

public class BigEnemyEffect : MonoBehaviour {
    public Direction direction_ = Direction.None;
}
