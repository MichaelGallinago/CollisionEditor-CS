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


        private void CanvasForRectanglesIsMouseOver(object sender, MouseButtonEventArgs e)
        {
            if ((this.DataContext as MainViewModel).window.canvasForRectangles.IsMouseOver)
                (this.DataContext as MainViewModel).window.canvasForRectangles.Opacity += 0.05;
            else
                (this.DataContext as MainViewModel).window.canvasForRectangles.Opacity -= 0.05;
            (this.DataContext as MainViewModel).window.canvasForRectangles.Opacity = (this.DataContext as MainViewModel).window.canvasForRectangles.Opacity;
        }

        internal void DrawRedLine()
        {
            RedLineService.DrawRedLine(ref RedLine);
        }

        void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(IsGood);
        }
        bool IsGood(char c)
        {
            if (TextBoxHexAngle.Text.Length >= 4)
                return false;
            if (c >= '0' && c <= '9')
                return true;
            if (c >= 'A' && c <= 'F')
                return true;
            
            return false;
        }


        private void TextBoxHexAngle_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (TextBoxHexAngle.Text.Length<2)
                {
                    TextBoxHexAngle.Text = "0x00";
                }
                if (TextBoxHexAngle.Text.Substring(0, 2) != "0x")
                {
                    TextBoxHexAngle.Text = "0x00";
                }
            }
        }
    }
}