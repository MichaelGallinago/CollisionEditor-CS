using System;
using System.Runtime.CompilerServices;

namespace CollisionEditor.model
{
    internal static class AngleConstructor
    {
        public static Vector2<int> GetCorrectDotPosition(Vector2<double> position, int cellSize)
        {
            return new Vector2<int>(
                (int)Math.Floor(position.X) & -cellSize, 
                (int)Math.Floor(position.Y) & -cellSize);
        }
    }
}
