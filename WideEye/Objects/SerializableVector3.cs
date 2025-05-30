

using UnityEngine;

namespace WideEye.Objects;

public class SerializableVector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public SerializableVector3()
    {
        
    }

    public SerializableVector3(Vector3 vector3)
    {
        X = vector3.x;
        Y = vector3.y;
        Z = vector3.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(X, Y, Z);
    }
}