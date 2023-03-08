using CollisionEditor.model;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.ApplicationLifetimes;
using CollisionEditorCS.Views;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;


namespace CollisionEditor
{
    internal static class SquaresService
    {
        static MainWindow mainWindow = (MainWindow)((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;
        private static Rectangle GetRectangle(Color color)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 8;
            rect.Height = 8;
            rect.Fill = new SolidColorBrush(color);
            return rect;
        }

        public static void DrawSquare(Color color, Vector2<int> cordinats, SquareAndPosition squareAndPosition)
        {
            Rectangle square = GetRectangle(color);
            if (cordinats.X >= 128)
                cordinats.X = 120;
            if (cordinats.Y >= 128)
                cordinats.Y = 120;

            mainWindow.canvasForRectangles.Children.Remove(squareAndPosition.Square);

            squareAndPosition.Square = square;
            squareAndPosition.Position = cordinats;

            Canvas.SetLeft(square, cordinats.X);
            Canvas.SetTop(square, cordinats.Y);

            mainWindow.canvasForRectangles.Children.Add(square);
        }

    }
}