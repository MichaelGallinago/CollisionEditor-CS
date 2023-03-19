using CollisionEditor.model;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CollisionEditor
{
    internal class SquareAndPosition
    {
        public Rectangle Square { get; set; } = new Rectangle();
        public Vector2<int> Position { get; set; } = new Vector2<int>();
        public Color Color { get; set; }

        public SquareAndPosition(Color color)
        {
            Color = color;
            Square.Fill = new SolidColorBrush(Color);
        }
    }
}
