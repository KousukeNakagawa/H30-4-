using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelManager : MonoBehaviour
{
    [SerializeField] GameObject frontRightWheel;
    [SerializeField] GameObject frontLeftWheel;
    List<GameObject> frontWheel = new List<GameObject>();
    GameObject player;

    [SerializeField, Range(10, 300)] float rotateSpeed = 120;
    [SerializeField, Range(10, 90)] float maxRotate = 45;
    [SerializeField, Range(-10, -90)] float minRotate = -45;

    float curve;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        frontWheel.Add(frontRightWheel);
        frontWheel.Add(frontLeftWheel);
    }

    void Update()
    {
        Handling();
        Curve();
    }

    void Handling()
    {
        //curve = Input.GetAxis("Curve");
    }

    void Curve()
    {
        foreach (var wheel in frontWheel)
        {
            //wheel.transform.eulerAngles += new Vector3(0, curve) * rotateSpeed * Time.deltaTime;
            wheel.transform.localEulerAngles += new Vector3(0, curve) * rotateSpeed * Time.deltaTime;
            //wheel.transform.Rotate(new Vector3(0.0f, curve) * rotateSpeed * Time.deltaTime);

            //-180＜上下の動き＜180に変更
            float angle = (180 <= wheel.transform.eulerAngles.y) ?
                wheel.transform.eulerAngles.y - 360 : wheel.transform.eulerAngles.y;

            var playerAnglesY = player.transform.eulerAngles.y;

            var playerY = (Mathf.Abs(playerAnglesY) >= 180) ?
                player.transform.eulerAngles.y - 360 : player.transform.eulerAngles.y;

            var playerAngle = (playerY < -120) ? playerY + 120 : playerY;

            //制限
            wheel.transform.eulerAngles =
                new Vector3(wheel.transform.eulerAngles.x, Mathf.Clamp(angle, minRotate + playerAnglesY, maxRotate + playerAnglesY), wheel.transform.eulerAngles.z);
        }
    }

    public Vector3 GetForward()
    {
        return frontWheel[0].transform.forward;
    }

    public float GetAngle()
    {
        return frontWheel[0].transform.localEulerAngles.y;
    }
}