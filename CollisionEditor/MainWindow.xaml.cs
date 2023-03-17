using System.Windows;
using System.Windows.Input;
using CollisionEditor.model;
using CollisionEditor.viewModel;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Linq;
using System;
using System.Threading.Tasks;

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
            var mousePosition = e.GetPosition(RectanglesGrid);
            Vector2<int> position = GetGridPosition(mousePosition, RectanglesGrid);
            //Vector2<int> cordinats = (this.DataContext as MainViewModel).GetCordinats(Mouse.GetPosition(canvasForRectangles).X, Mouse.GetPosition(canvasForRectangles).Y);

            SquaresService.DrawSquare(Colors.Blue, position, BlueAndGreenSquare.Item1);
            
            if (BlueAndGreenSquare.Item1.Square != null & BlueAndGreenSquare.Item2.Square != null)
            {
                (this.DataContext as MainViewModel).AngleUpdator(BlueAndGreenSquare.Item1.Position, BlueAndGreenSquare.Item2.Position);

                DrawRedLine();
            }
        }

        private Vector2<int> GetGridPosition(Point mousePosition, Grid grid)
        {
            Vector2<int> position = new Vector2<int>();

            foreach (var column in grid.ColumnDefinitions)
            {
                if (mousePosition.X > column.Offset && mousePosition.X < (column.Offset + column.ActualWidth))
                    break;
                position.X++;
            }

            foreach (var row in grid.ColumnDefinitions)
            {
                if (mousePosition.Y > row.Offset && mousePosition.Y < (row.Offset + row.ActualWidth))
                    break;
                position.Y++;
            }

            return position;
        }

        private void CanvasForRectanglesMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(RectanglesGrid);
            Vector2<int> position = GetGridPosition(mousePosition, RectanglesGrid);
            //Vector2<int> cordinats = (this.DataContext as MainViewModel).GetCordinats(Mouse.GetPosition(canvasForRectangles).X, Mouse.GetPosition(canvasForRectangles).Y);

            SquaresService.DrawSquare(Colors.Green, position, BlueAndGreenSquare.Item2);

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
            double actualWidth = ActualWidth / 587 * 26;
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


            ByteAngleIncrimentButton.Height = actualHeight/2;
            ByteAngleIncrimentButton.Width = actualWidth;
            ByteAngleDecrementButton.Height = actualHeight/2 -1;
            ByteAngleDecrementButton.Width = actualWidth;

            HexAngleIncrimentButton.Height = actualHeight / 2;
            HexAngleIncrimentButton.Width = actualWidth - 1;
            HexAngleDecrementButton.Height = actualHeight / 2 - 1;
            HexAngleDecrementButton.Width = actualWidth - 1;

            FullAngleIncrimentButton.Height = actualHeight / 2;
            FullAngleIncrimentButton.Width = actualWidth-1;
            FullAngleDecrementButton.Height = actualHeight / 2 - 1;
            FullAngleDecrementButton.Width = actualWidth-1;


            SelectTileTextBox.Height = actualHeight - 2;
            SelectTileTextBox.FontSize = actualFontSize;
            SelectTileButton.Height = actualHeight - 2;
            SelectTileButton.FontSize = actualFontSize;
        }
    }
}