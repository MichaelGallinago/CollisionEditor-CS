using System.Windows;
using System.Windows.Input;
using CollisionEditor.model;
using CollisionEditor.viewModel;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Runtime.Intrinsics.Arm;

namespace CollisionEditor
{   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this);
        }


        (SquareAndPosition, SquareAndPosition) BlueAndGreenSquare = (new SquareAndPosition(), new SquareAndPosition());
        Line RedLine = new Line();

        private void CanvasForRectanglesMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Vector2<int> cordinats = (this.DataContext as MainViewModel).GetCordinats(Mouse.GetPosition(canvasForRectangles).X, Mouse.GetPosition(canvasForRectangles).Y);
            
            SquaresService.DrawSquare(Colors.Blue, cordinats, BlueAndGreenSquare.Item1);
            
            if (BlueAndGreenSquare.Item1.Square != null & BlueAndGreenSquare.Item2.Square != null)
            {
                (this.DataContext as MainViewModel).AngleUpdator(BlueAndGreenSquare.Item1.Position, BlueAndGreenSquare.Item2.Position);

                DrawRedLine();
            }
        }

        private void CanvasForRectanglesMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Vector2<int> cordinats = (this.DataContext as MainViewModel).GetCordinats(Mouse.GetPosition(canvasForRectangles).X, Mouse.GetPosition(canvasForRectangles).Y);

            SquaresService.DrawSquare(Colors.Green, cordinats, BlueAndGreenSquare.Item2);

            if (BlueAndGreenSquare.Item1.Square != null & BlueAndGreenSquare.Item2.Square != null)
            {
                (this.DataContext as MainViewModel).AngleUpdator(BlueAndGreenSquare.Item1.Position, BlueAndGreenSquare.Item2.Position);

                DrawRedLine();
            }
        }

        internal void DrawRedLine()
        {
            RedLineService.DrawRedLine(ref RedLine);
        }

        private void TextBoxHexAngle_KeyDown(object sender, KeyEventArgs e)
        {
            bool isCtrl = (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl));
            Key[] exceptions = new Key[] { Key.Back, Key.Delete, Key.Left, Key.Right };
            if (TextBoxHexAngle.Text.Length >= 4 && !exceptions.Contains(e.Key) && !isCtrl 
                || TextBoxHexAngle.Text.Length > 0 && e.Key == Key.C && isCtrl)
                e.Handled = true;
        }

        private bool _inRectangleCanvas = false;

        private async void canvasForRectanglesUpdate(bool isAppear)
        {
            _inRectangleCanvas = isAppear;
            while (isAppear && canvasForRectangles.Opacity < 1d || !isAppear && canvasForRectangles.Opacity > 0d)
            {
                if (_inRectangleCanvas != isAppear)
                    return;

                await Task.Delay(10);
                canvasForRectangles.Opacity = Math.Clamp(canvasForRectangles.Opacity + (isAppear ? 0.05 : -0.05), 0d, 1d);
            }
        }

        private void canvasForRectangles_MouseEnter(object sender, MouseEventArgs e)
        {
            canvasForRectanglesUpdate(true);
        }

        private void canvasForRectangles_MouseLeave(object sender, MouseEventArgs e)
        {
            canvasForRectanglesUpdate(false);
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {   
            double actualHeight = ActualHeight / 424 * 20;
            double actualFontSize = (25.4 / 96 * actualHeight) / 0.35 - 4;

            Heights.Height = actualHeight;
            Heights.FontSize = actualFontSize;

            Widths.Height = actualHeight;
            Widths.FontSize = actualFontSize;
            
            TextBlockFullAngle.Height = actualHeight;
            TextBlockFullAngle.FontSize = actualFontSize;
            TextBoxByteAngle.Height = actualHeight;
            TextBoxByteAngle.FontSize = actualFontSize;
            TextBoxHexAngle.Height = actualHeight;
            TextBoxHexAngle.FontSize = actualFontSize;

            SelectTileTextBox.Height = actualHeight - 2;
            SelectTileTextBox.FontSize = actualFontSize;
            SelectTileButton.Height = actualHeight - 2;
            SelectTileButton.FontSize = actualFontSize;
        }
    }
}