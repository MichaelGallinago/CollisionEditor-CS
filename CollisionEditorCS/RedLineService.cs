using CollisionEditorCS.Views;
using Avalonia;
using Avalonia.Media;
using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using System;
using Avalonia.Controls.ApplicationLifetimes;

namespace CollisionEditor
{
    internal static class RedLineService
    {
        static MainWindow mainWindow = (MainWindow)((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;

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
            line.RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Absolute);

            Canvas.SetTop(line, 64);
            line.RenderTransform = rotateTransform1;

            mainWindow.canvasForLine.Children.Remove(redLine);
            redLine = line;
            mainWindow.canvasForLine.Children.Add(line);
        }
    }
}