using System.Windows;
using System.Windows.Input;
using CollisionEditor.model;
using CollisionEditor.viewModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System;
using System.Windows.Media.Media3D;
using System.Runtime.CompilerServices;

namespace CollisionEditor
{
    internal static class SquaresService
    {
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
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

            Canvas.SetLeft(square, cordinats.X);
            Canvas.SetTop(square, cordinats.Y);
            
            
            if (cordinats != squareAndPosition.Position)
            {
                mainWindow.canvasForRectangles.Children.Add(square);
            }
            
            squareAndPosition.Square = square;
            squareAndPosition.Position = cordinats;
        }

    }
}