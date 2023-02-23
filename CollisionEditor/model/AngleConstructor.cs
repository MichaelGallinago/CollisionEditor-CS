using System;

namespace CollisionEditor.model
{
    static internal class AngleConstructor
    {
        const int cellSize = 8;
        
        public static Vector2<int> GetCorrectDotPosition(Vector2<double> position)
        {
            return new Vector2<int>(
                (int)Math.Round(position.X) & -cellSize, 
                (int)Math.Round(position.Y) & -cellSize);
        }
    }
}
