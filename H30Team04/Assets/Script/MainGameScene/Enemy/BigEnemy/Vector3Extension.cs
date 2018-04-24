using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 GetUnityVector3(this Vector3 self)
    {
        if (self.x > 180.0f) self.x -= 360.0f;
        else if (self.x < -180.0f) self.x += 360.0f;
        if (self.y > 180.0f) self.y -= 360.0f;
        else if (self.y < -180.0f) self.y += 360.0f;
        if (self.z > 180.0f) self.z -= 360.0f;
        else if (self.z < -180.0f) self.z += 360.0f;

        return self;
    }
}
