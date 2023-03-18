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

        private void RectanglesGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void RectanglesGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {   
            var mousePosition = e.GetPosition(RectanglesGrid);
            Vector2<int> position = GetGridPosition(mousePosition, RectanglesGrid);

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

        private void SelectTileTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                (this.DataContext as MainViewModel).SelectTile();
        }

        private void TextBoxHexAngle_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool isCtrl = (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl));
            Key[] exceptions = new Key[] { Key.Back, Key.Delete, Key.Left, Key.Right };
            if (TextBoxHexAngle.Text.Length >= 4 && !exceptions.Contains(e.Key) && !isCtrl 
                || TextBoxHexAngle.Text.Length > 0 && e.Key == Key.C && isCtrl)
                e.Handled = true;
        }

        private bool _inRectanglesGrid = false;

        private async void RectanglesGridUpdate(bool isAppear)
        {
            _inRectanglesGrid = isAppear;
            while (isAppear && RectanglesGrid.Opacity < 1d || !isAppear && RectanglesGrid.Opacity > 0d)
            {
                if (_inRectanglesGrid != isAppear)
                    return;

                await Task.Delay(10);
                RectanglesGrid.Opacity = Math.Clamp(RectanglesGrid.Opacity + (isAppear ? 0.05 : -0.05), 0d, 1d);
            }
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
            
            TextBlockFullAngle.Height = actualHeightTextAndButtons - 2;
            TextBlockFullAngle.FontSize = actualFontSize;
            TextBoxByteAngle.Height = actualHeightTextAndButtons - 2;
            TextBoxByteAngle.FontSize = actualFontSize;
            TextBoxHexAngle.Height = actualHeightTextAndButtons - 2;
            TextBoxHexAngle.FontSize = actualFontSize;


            ByteAngleIncrimentButton.Height = actualHeightTextAndButtons/2;
            ByteAngleIncrimentButton.Width = actualWidthTextAndButtons - 2;          
            TriangleUpByteAngle.Height = actualWidthTextAndButtons / 2 - 3 ;
            TriangleUpByteAngle.Width = actualWidthTextAndButtons /2 - 3 ;

            ByteAngleDecrementButton.Height = actualHeightTextAndButtons/2 -1;
            ByteAngleDecrementButton.Width = actualWidthTextAndButtons - 2;
            TriangleDownByteAngle.Height = actualWidthTextAndButtons / 2 - 3;
            TriangleDownByteAngle.Width = actualWidthTextAndButtons / 2 - 3;

            HexAngleIncrimentButton.Height = actualHeightTextAndButtons / 2;
            HexAngleIncrimentButton.Width = actualWidthTextAndButtons - 2;
            TriangleUpHexAngle.Height = actualWidthTextAndButtons / 2 - 3;
            TriangleUpHexAngle.Width = actualWidthTextAndButtons / 2 - 3;

            HexAngleDecrementButton.Height = actualHeightTextAndButtons / 2 - 1;
            HexAngleDecrementButton.Width = actualWidthTextAndButtons - 2;
            TriangleDownHexAngle.Height = actualWidthTextAndButtons / 2 - 3;
            TriangleDownHexAngle.Width = actualWidthTextAndButtons / 2 - 3;

            SelectTileTextBox.Height = actualHeightTextAndButtons - 2;
            SelectTileTextBox.FontSize = actualFontSize;
            SelectTileButton.Height = actualHeightTextAndButtons - 2;
            SelectTileButton.FontSize = actualFontSize;

            DrawRedLine();
        }
    }
}