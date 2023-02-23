using System;

namespace CollisionEditor.model
{
    static internal class AngleConstructor
    {
        const int cellSize = 8;
<<<<<<< HEAD
        public static Vector2<int> GetCorrectDotPosition(Vector2<double> position)
=======

        public Vector2<int> GetCorrectDotPosition(Vector2<double> position)
>>>>>>> develop
        {
            return new Vector2<int>(
                (int)Math.Round(position.X) & -cellSize, 
                (int)Math.Round(position.Y) & -cellSize);
        }
    }
}
