using System.Windows;
using System.Windows.Input;
using CollisionEditor.model;
using CollisionEditor.viewModel;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;

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
            Key[] exceptions = new Key[] { Key.Back, Key.Delete, Key.Left, Key.Right };
            if (TextBoxHexAngle.Text.Length >= 4 && !exceptions.Contains(e.Key))
                e.Handled = true;
        }

        private void canvasForRectangles_MouseLeave(object sender, MouseEventArgs e)
        {
            canvasForRectangles.Opacity= 0;
        }

        private void canvasForRectangles_MouseEnter(object sender, MouseEventArgs e)
        {
            canvasForRectangles.Opacity = 0.5;
        }

        private void TextBoxHexAngle_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}