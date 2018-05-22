using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 GetUnityVector3(this Vector3 self)
    {  //回転の計算処理
        if (self.x > 180.0f) self.x -= 360.0f;
        else if (self.x < -180.0f) self.x += 360.0f;
        if (self.y > 180.0f) self.y -= 360.0f;
        else if (self.y < -180.0f) self.y += 360.0f;
        if (self.z > 180.0f) self.z -= 360.0f;
        else if (self.z < -180.0f) self.z += 360.0f;

        return self;
    }

    public static Vector2 ToTopView(this Vector3 self)
    {
        return new Vector2(self.x, self.z);
    }
}
