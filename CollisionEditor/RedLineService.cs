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

        public static void DrawRedLine(ref Line redLine)
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

            mainWindow.canvasForLine.Children.Remove(redLine);
            redLine = newLine;
            mainWindow.canvasForLine.Children.Add(newLine);
        }
    }
}