using System.Windows;
using System.Windows.Input;
using CollisionEditor.model;
using CollisionEditor.viewModel;
using System.Windows.Media;
using System.Windows.Shapes;
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

        private static bool IsOnTheCanvas(System.Windows.Controls.Canvas canvasForRectangles)
        {   
            if (Mouse.GetPosition(canvasForRectangles).X >= 0 && Mouse.GetPosition(canvasForRectangles).X <= 128 && Mouse.GetPosition(canvasForRectangles).Y >= 0 && Mouse.GetPosition(canvasForRectangles).Y <= 128)
            {
                return true;
            }
            return false;
        }
        
        private async void canvasForRectangles_MouseLeave(object sender, MouseEventArgs e)
        {
            await Task.Delay(1000);
            while (canvasForRectangles.Opacity>=0.1 &&  !IsOnTheCanvas(canvasForRectangles))
            {
                await Task.Delay(100);
                 canvasForRectangles.Opacity -= 0.1;
            }
        }
        private void canvasForRectangles_MouseEnter(object sender, MouseEventArgs e)
        {   
            canvasForRectangles.Opacity = 1;
        }

        private void TextBoxHexAngle_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}