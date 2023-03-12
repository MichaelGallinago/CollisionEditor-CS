using System.Windows;
using System.Windows.Input;
using CollisionEditor.model;
using CollisionEditor.viewModel;
using System.Windows.Media;
using System.Windows.Shapes;

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
    }
}