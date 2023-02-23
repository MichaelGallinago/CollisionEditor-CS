using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollisionEditor.model
{
    internal class AngleConstructor
    {
        const int cellSize = 8;
        public Vector2<int> GetCorrectDotPosition(Vector2<double> position)
        {
            return new Vector2<int>(
                (int)Math.Round(position.X) & -cellSize, 
                (int)Math.Round(position.Y) & -cellSize);
        }
    }
}
