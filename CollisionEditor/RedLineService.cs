using System.Windows;
using System.Windows.Input;
using CollisionEditor.model;
using CollisionEditor.viewModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.LinkLabel;

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

            Line newLine = new Line();
            double length = 64 / Math.Abs(Math.Cos((-45 + ((floatAngle + 45) % 90)) / 180 * Math.PI));
            floatAngle += 90;
            newLine.X1 = length * Math.Sin(floatAngle / 180 * Math.PI);
            newLine.Y1 = length * Math.Cos(floatAngle / 180 * Math.PI);
            newLine.X2 = -newLine.X1;
            newLine.Y2 = -newLine.Y1;
            Canvas.SetTop(newLine, 64);
            Canvas.SetLeft(newLine, 64);
            newLine.Stroke = new SolidColorBrush(Colors.Red);
            newLine.Fill = new SolidColorBrush(Colors.Red);
            mainWindow.canvasForLine.Children.Add(newLine);
            /*
            Rectangle line = new Rectangle();
            line.Width = 128 / Math.Abs(Math.Cos((-45 + ((floatAngle + 45) % 90)) / 180 * Math.PI));
            line.Height = 1;
            line.Fill = new SolidColorBrush(Colors.Red);
            

            RotateTransform rotateTransform1 = new RotateTransform(180 - floatAngle,64,0);
            //line.RenderTransformOrigin = new Point(0.5 * (128 * Math.Sqrt(2) / line.Width), 0);

            Canvas.SetTop(line, 64);
            line.RenderTransform = rotateTransform1;

            mainWindow.canvasForLine.Children.Remove(redLine);
            redLine = line;
            mainWindow.canvasForLine.Children.Add(line);
            */
        }
    }
}