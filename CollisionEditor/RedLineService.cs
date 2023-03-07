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
    internal class RedLineService
    {
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

        public static void DrawRedLine(ref Rectangle redLine)
        { 
            Rectangle line = new Rectangle();
            line.Width = 128 * Math.Sqrt(2);
            line.Height = 1;
            line.Fill = new SolidColorBrush(Colors.Red);

            string stringAngle = mainWindow.TextBlockFullAngle.Text.TrimEnd('\'');
            float floatAngle = float.Parse(stringAngle);
            if (floatAngle > 180)
            {
                floatAngle = floatAngle - 180;
            }
            RotateTransform rotateTransform1 = new RotateTransform(180 - floatAngle);
            line.RenderTransformOrigin = new Point(0.5, 0.5);

            Canvas.SetTop(line, 64);
            line.RenderTransform = rotateTransform1;

            mainWindow.canvasForLine.Children.Remove(redLine);
            redLine = line;
            mainWindow.canvasForLine.Children.Add(line);
        }
    }
}