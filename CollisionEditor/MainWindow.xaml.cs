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
    
    public partial class MainWindow : Window
    {

        (SquareAndPosition, SquareAndPosition) BlueAndGreenSquare = (new SquareAndPosition(), new SquareAndPosition());
        Rectangle RedLine = new Rectangle();
        private Rectangle GetRectangle(Color color)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 8;
            rect.Height = 8;
            rect.Fill = new SolidColorBrush(color);
            return rect;
        }
        private void DrawRedLine()
        {
            Rectangle line = new Rectangle();
            line.Width = 128 * Math.Sqrt(2);
            line.Height = 1;
            line.Fill = new SolidColorBrush(Colors.Red);
            string stringAngle = TextBlock360Angle.Text.TrimEnd('\'');
            float floatAngle = float.Parse(stringAngle);
            if (floatAngle > 180)
            {
                floatAngle = floatAngle - 180;
            }
            RotateTransform rotateTransform1 = new RotateTransform(180 - floatAngle);
            line.RenderTransformOrigin = new Point(0.5, 0.5);

            Canvas.SetTop(line, 64);
            line.RenderTransform = rotateTransform1;

            canvasForLine.Children.Remove(RedLine);
            RedLine = line;
            canvasForLine.Children.Add(line);
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this);
        }


        private void ImageOfTileGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

            Vector2<int> cordinats = (this.DataContext as MainViewModel).GetCordinats(Mouse.GetPosition(mainWindow.ImageOfTileGrid).X, Mouse.GetPosition(mainWindow.ImageOfTileGrid).Y);
            if (cordinats.X >= 128)
                cordinats.X = 120;
            if (cordinats.Y >= 128)
                cordinats.Y = 120;

            Rectangle rect = GetRectangle(Colors.Blue);

            canvasForRectangles.Children.Remove(BlueAndGreenSquare.Item1.Square);
            
            BlueAndGreenSquare.Item1.Square = rect;
            BlueAndGreenSquare.Item1.Position = cordinats;

            Canvas.SetLeft(rect, cordinats.X);
            Canvas.SetTop(rect, cordinats.Y);

            canvasForRectangles.Children.Add(rect);
            if (BlueAndGreenSquare.Item1 != null & BlueAndGreenSquare.Item2 != null)
            {
                (this.DataContext as MainViewModel).AngleUpdator(BlueAndGreenSquare.Item1.Position, BlueAndGreenSquare.Item2.Position);

                DrawRedLine();
            }
        }
        
        private void ImageOfTileGridMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

            Vector2<int> cordinats = (this.DataContext as MainViewModel).GetCordinats(Mouse.GetPosition(mainWindow.ImageOfTileGrid).X, Mouse.GetPosition(mainWindow.ImageOfTileGrid).Y);
            
            if (cordinats.X >= 128)
                cordinats.X = 120;
            if (cordinats.Y >= 128)
                cordinats.Y = 120;

            Rectangle rect = GetRectangle(Colors.Green);

            canvasForRectangles.Children.Remove(BlueAndGreenSquare.Item2.Square);
            BlueAndGreenSquare.Item2.Square = rect;
            BlueAndGreenSquare.Item2.Position = cordinats;

            Canvas.SetLeft(rect, cordinats.X);
            Canvas.SetTop(rect, cordinats.Y);

            canvasForRectangles.Children.Add(rect);

            if (BlueAndGreenSquare.Item1 != null & BlueAndGreenSquare.Item2 != null)
            {
                (this.DataContext as MainViewModel).AngleUpdator(BlueAndGreenSquare.Item1.Position, BlueAndGreenSquare.Item2.Position);

                DrawRedLine();
            }
        }

        

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}