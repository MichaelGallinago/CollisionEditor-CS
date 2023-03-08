using System.Windows;
using System.Windows.Input;
using CollisionEditor.model;
using CollisionEditor.viewModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System;
using System.Windows.Media.Media3D;
namespace CollisionEditor
{
    internal static class RedLineService
    {
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

        public static void DrawRedLine(ref Rectangle redLine)
        {
            string stringAngle = mainWindow.TextBlockFullAngle.Text.TrimEnd('\'');
            float floatAngle = float.Parse(stringAngle);
            if (floatAngle > 180)
            {
                floatAngle = floatAngle - 180;
            }

            Rectangle line = new Rectangle();
            line.Width = 128 * (1 + (Math.Sqrt(2) - 1) * Math.Abs(Math.Sin(floatAngle * 2 / 180 * Math.PI)));
            line.Height = 1;
            line.Fill = new SolidColorBrush(Colors.Red);

            RotateTransform rotateTransform1 = new RotateTransform(180 - floatAngle);
            line.RenderTransformOrigin = new Point(0.5 * (128 * Math.Sqrt(2) / line.Width), 0.5);

            Canvas.SetTop(line, 64);
            line.RenderTransform = rotateTransform1;

            mainWindow.canvasForLine.Children.Remove(redLine);
            redLine = line;
            mainWindow.canvasForLine.Children.Add(line);
        }
    }
}