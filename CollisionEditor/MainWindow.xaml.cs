using CollisionEditor.ViewModel;
using CollisionEditor.Model;
using CollisionEditor.View;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows;
using System.Linq;
using System;


namespace CollisionEditor
{   
    public partial class MainWindow : Window
    {
        public int LastChosenTile { get; set; }

        private const int tileMapSeparation = 4;
        private const int tileMapTileScale  = 2;

        private bool mouseInRectanglesGrid = false;
        private (SquareAndPosition, SquareAndPosition) blueAndGreenSquare = (new SquareAndPosition(Colors.Blue), new SquareAndPosition(Colors.Green));
        private Line redLine = new Line();

        private MainViewModel windowViewModel { get; set; }

        public MainWindow()
        {   
            InitializeComponent();
            windowViewModel = new MainViewModel(this);
            DataContext = windowViewModel;
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

            foreach (var row in grid.RowDefinitions)
            {
                if (mousePosition.Y > row.Offset && mousePosition.Y < (row.Offset + row.ActualHeight))
                    break;
                position.Y++;
            }

            return position;
        }

        private void RectanglesGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RectanglesGridUpdate(e, blueAndGreenSquare.Item1, blueAndGreenSquare.Item2);
        }

        private void RectanglesGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            RectanglesGridUpdate(e, blueAndGreenSquare.Item2, blueAndGreenSquare.Item1);
        }

        private void RectanglesGridUpdate(MouseButtonEventArgs e, SquareAndPosition firstSquare, SquareAndPosition secondSquare) 
        {
            if (windowViewModel.AngleMap.Values.Count <= 0)
                return;

            var mousePosition = e.GetPosition(RectanglesGrid);
            Vector2<int> position = GetGridPosition(mousePosition, RectanglesGrid);

            SquaresService.MoveSquare(position, firstSquare, secondSquare);

            if (RectanglesGrid.Children.Contains(firstSquare.Square) && RectanglesGrid.Children.Contains(secondSquare.Square))
            {
                windowViewModel.UpdateAngles(blueAndGreenSquare.Item1.Position, blueAndGreenSquare.Item2.Position);
                DrawRedLine();
            }
        }

        internal void DrawRedLine()
        {
            if (windowViewModel.AngleMap.Values.Count > 0)
                RedLineService.DrawRedLine(ref redLine);
        }

        private void SelectTileTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                windowViewModel.SelectTile();
        }

        private void TextBoxHexAngle_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool isCtrlKeyDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            Key[] exceptions = new Key[] { Key.Back, Key.Delete, Key.Left, Key.Right };

            if (TextBoxHexAngle.Text.Length >= 4 && !exceptions.Contains(e.Key) && !isCtrlKeyDown
                || TextBoxHexAngle.Text.Length > 0 && e.Key == Key.C && isCtrlKeyDown)
            {
                e.Handled = true;
            }
        }

        private async void RectanglesGridUpdate(bool isAppear)
        {
            mouseInRectanglesGrid = isAppear;
            while (isAppear && RectanglesGrid.Opacity < 1d || !isAppear && RectanglesGrid.Opacity > 0d)
            {
                if (mouseInRectanglesGrid != isAppear)
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
        private uint GetUniformGridIndex(Point mousePosition, System.Windows.Controls.Primitives.UniformGrid grid)
        {
            return (uint)mousePosition.X / 36 + ((uint)mousePosition.Y / 36)* (uint)TileMapGrid.Columns;
        }

        
        private void TileMapGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (windowViewModel.TileSet.Tiles.Count <= 0)
                return;

            Image lastTile = windowViewModel.GetTile((int)windowViewModel.ChosenTile);

            TileMapGrid.Children.RemoveAt((int)windowViewModel.ChosenTile);
            TileMapGrid.Children.Insert((int)windowViewModel.ChosenTile, lastTile);

            var mousePosition = e.GetPosition(TileMapGrid);

            windowViewModel.ChosenTile = GetUniformGridIndex(mousePosition, TileMapGrid);

            if (windowViewModel.ChosenTile > windowViewModel.TileSet.Tiles.Count-1)
                windowViewModel.ChosenTile = (uint)windowViewModel.TileSet.Tiles.Count - 1;
            
            Image newTile = windowViewModel.GetTile((int)windowViewModel.ChosenTile);

            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Red);
            border.BorderThickness = new Thickness(2);
            border.Child = newTile;

            var tileSize = windowMain.TileSet.TileSize;
            border.Width = 36;
            border.Height = 36;
            border.Child = newTile;
            
            TileMapGrid.Children.RemoveAt((int)windowViewModel.ChosenTile);
            TileMapGrid.Children.Insert((int)windowViewModel.ChosenTile, border);

            windowViewModel.SelectTileFromTileMap();
            LastChosenTile = (int)windowViewModel.ChosenTile;
        }
        private const int menyHeight = 20;
        private const int countHeightParts = 404;
        private const int countWidthParts = 587;
        private const int baseTileMapGridWidth = 288;
        
        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            int countOfTiles = windowViewModel.TileSet.Tiles.Count;
            var tileSize     = windowViewModel.TileSet.TileSize;

            double actualHeightTextAndButtons = (ActualHeight - menyHeight) / countHeightParts * 20;
            double actualWidthUpAndDownButtons = ActualWidth / countWidthParts * 23;
            double actualFontSize = Math.Min((25.4 / 96 * actualHeightTextAndButtons) / 0.35 - 4, (25.4 / 96 * (ActualWidth / countHeightParts * 43)) / 0.35 - 21);

            double actualHeightGrid = (ActualHeight - menyHeight) / countHeightParts * 128;

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


            ByteAngleIncrimentButton.Height = actualHeightTextAndButtons / 2;
            ByteAngleIncrimentButton.Width = actualWidthUpAndDownButtons - 3; 
            ByteAngleDecrementButton.Height = actualHeightTextAndButtons / 2 - 1;
            ByteAngleDecrementButton.Width = actualWidthUpAndDownButtons - 3;

            TriangleUpByteAngle.Height = actualHeightTextAndButtons / 2 - 5;
            TriangleUpByteAngle.Width = actualWidthUpAndDownButtons / 2 - 5;
            TriangleDownByteAngle.Height = actualHeightTextAndButtons / 2 - 5;
            TriangleDownByteAngle.Width = actualWidthUpAndDownButtons / 2 - 5;

            HexAngleIncrimentButton.Height = actualHeightTextAndButtons / 2;
            HexAngleIncrimentButton.Width = actualWidthUpAndDownButtons - 3;
            HexAngleDecrementButton.Height = actualHeightTextAndButtons / 2 - 1;
            HexAngleDecrementButton.Width = actualWidthUpAndDownButtons - 3;

            TriangleUpHexAngle.Height = actualHeightTextAndButtons / 2 - 5;
            TriangleUpHexAngle.Width = actualWidthUpAndDownButtons / 2 - 5;
            TriangleDownHexAngle.Height = actualHeightTextAndButtons / 2 - 5;
            TriangleDownHexAngle.Width = actualWidthUpAndDownButtons / 2 - 5;

            int tileWidth  = tileSize.Width  * tileMapTileScale;
            int tileHeight = tileSize.Height * tileMapTileScale;

            TileMapGrid.Width = baseTileMapGridWidth + (((int)(ActualWidth / countWidthParts * 278) - 314) / tileWidth) * tileWidth;
            TileMapGrid.Columns = ((int)TileMapGrid.Width + tileMapSeparation) / (tileWidth + tileMapSeparation);

            TileMapGrid.Height = (int)Math.Ceiling((double)countOfTiles / TileMapGrid.Columns) * (tileHeight + tileMapSeparation);

            SelectTileTextBox.Height = actualHeightTextAndButtons - 2;
            SelectTileTextBox.FontSize = actualFontSize;
            SelectTileButton.Height = actualHeightTextAndButtons - 2;
            SelectTileButton.FontSize = actualFontSize;

            DrawRedLine();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = "https://youtu.be/m5sbRiwQPMQ?t=87",
                UseShellExecute = true,
            });
        }
    }
}
