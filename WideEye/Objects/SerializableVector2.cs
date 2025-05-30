using UnityEngine;

namespace WideEye.Objects;

public class SerializableVector2
{
    public float X { get; set; }
    public float Y { get; set; }

    public SerializableVector2()
    {
        
    }

    public SerializableVector2(Vector2 vector2)
    {
        X = vector2.x;
        Y = vector2.y;
    }
    
    public Vector2 ToVector2()
    {
        return new Vector2(X, Y);
    }
}