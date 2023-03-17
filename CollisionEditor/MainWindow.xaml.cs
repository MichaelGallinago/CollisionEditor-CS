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

        private async void RectanglesGridUpdate(bool isAppear)
        {
            //_inRectangleCanvas = isAppear;
            //while (isAppear && canvasForRectangles.Opacity < 1d || !isAppear && canvasForRectangles.Opacity > 0d)
            //{
            //    if (_inRectangleCanvas != isAppear)
            //        return;

            //    await Task.Delay(10);
            //    canvasForRectangles.Opacity = Math.Clamp(canvasForRectangles.Opacity + (isAppear ? 0.05 : -0.05), 0d, 1d);
            //}
        }

        private void RectanglesGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            RectanglesGridUpdate(true);
        }

        private void RectanglesGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            RectanglesGridUpdate(false);
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {   
            double actualHeightTextAndButtons = ActualHeight / 424 * 20;
            double actualWidthTextAndButtons = ActualWidth / 587 * 26;
            double actualFontSize = (25.4 / 96 * actualHeightTextAndButtons) / 0.35 - 4;

            double actualHeightGrid = ActualHeight / 424 * 128;
            
            TileGrid.Width = actualHeightGrid;
            TileGrid.Height = actualHeightGrid;

            RectanglesGrid.Width = actualHeightGrid;
            RectanglesGrid.Height = actualHeightGrid;

            canvasForLine.Width = actualHeightGrid;
            canvasForLine.Height = actualHeightGrid;

            Heights.Height = actualHeightTextAndButtons;
            Heights.FontSize = actualFontSize;

            Widths.Height = actualHeightTextAndButtons;
            Widths.FontSize = actualFontSize;
            
            TextBlockFullAngle.Height = actualHeightTextAndButtons;
            TextBlockFullAngle.FontSize = actualFontSize;
            TextBoxByteAngle.Height = actualHeightTextAndButtons;
            TextBoxByteAngle.FontSize = actualFontSize;
            TextBoxHexAngle.Height = actualHeightTextAndButtons;
            TextBoxHexAngle.FontSize = actualFontSize;


            ByteAngleIncrimentButton.Height = actualHeightTextAndButtons/2;
            ByteAngleIncrimentButton.Width = actualWidthTextAndButtons;
            ByteAngleDecrementButton.Height = actualHeightTextAndButtons/2 -1;
            ByteAngleDecrementButton.Width = actualWidthTextAndButtons;

            HexAngleIncrimentButton.Height = actualHeightTextAndButtons / 2;
            HexAngleIncrimentButton.Width = actualWidthTextAndButtons - 1;
            HexAngleDecrementButton.Height = actualHeightTextAndButtons / 2 - 1;
            HexAngleDecrementButton.Width = actualWidthTextAndButtons - 1;

            FullAngleIncrimentButton.Height = actualHeightTextAndButtons / 2;
            FullAngleIncrimentButton.Width = actualWidthTextAndButtons-1;
            FullAngleDecrementButton.Height = actualHeightTextAndButtons / 2 - 1;
            FullAngleDecrementButton.Width = actualWidthTextAndButtons-1;


            SelectTileTextBox.Height = actualHeightTextAndButtons - 2;
            SelectTileTextBox.FontSize = actualFontSize;
            SelectTileButton.Height = actualHeightTextAndButtons - 2;
            SelectTileButton.FontSize = actualFontSize;
        }
    }
}