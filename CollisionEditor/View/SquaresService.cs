using CollisionEditor.model;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CollisionEditor
{
    internal static class SquaresService
    {
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

        public static void DrawSquare(Color color, Vector2<int> position, SquareAndPosition squareAndPosition)
        {
            Rectangle square = new Rectangle()
            {
                Fill = new SolidColorBrush(color)
            };

            mainWindow.RectanglesGrid.Children.Remove(squareAndPosition.Square);
            square.SetValue(Grid.ColumnProperty, position.X);
            square.SetValue(Grid.RowProperty,    position.Y);

            if (Equals(position, squareAndPosition.Position) && Equals(color, squareAndPosition.Color))
            {
                squareAndPosition.Square = null;
                squareAndPosition.Position = new Vector2<int>();
                squareAndPosition.Color = new Color();
            }
            else
            {
                mainWindow.RectanglesGrid.Children.Add(square);
                squareAndPosition.Square = square;
                squareAndPosition.Position = position;
                squareAndPosition.Color = color;
            }
            
        }
    }
}
