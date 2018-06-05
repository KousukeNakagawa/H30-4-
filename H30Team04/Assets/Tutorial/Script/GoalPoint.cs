using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    public static bool Goal { get; private set; }

    void Start()
    {
        Goal = false;
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            Goal = true;
    }
}
