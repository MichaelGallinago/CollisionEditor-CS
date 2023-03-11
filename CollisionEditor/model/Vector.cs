using System;
using System.Numerics;

namespace CollisionEditor.model
{
    public struct Vector2<T>
    {
        public T X { get; set; }
        public T Y { get; set; }

        public Vector2(T x, T y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Vector2<T> obj)
        {
            if ((X is null) || (Y is null) || (obj.X is null) || (obj.Y is null))
                return false;

            return X.Equals(obj.X) && Y.Equals(obj.Y);
        }
    }
}
