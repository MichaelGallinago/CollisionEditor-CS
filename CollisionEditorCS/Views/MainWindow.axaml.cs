using Avalonia.Controls;
using Avalonia.Media;
using CollisionEditor.model;
using CollisionEditorCS.ViewModels;

namespace CollisionEditorCS.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }

        (SquareAndPosition, SquareAndPosition) BlueAndGreenSquare = (new SquareAndPosition(), new SquareAndPosition());
        Rectangle RedLine = new Rectangle();

        private void ImageOfTileGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Vector2<int> cordinats = (this.DataContext as MainViewModel).GetCordinats(Mouse.GetPosition(ImageOfTileGrid).X, Mouse.GetPosition(ImageOfTileGrid).Y);

            SquaresService.DrawSquare(Colors.Blue, cordinats, BlueAndGreenSquare.Item1);

            if (BlueAndGreenSquare.Item1 != null & BlueAndGreenSquare.Item2 != null)
            {
                (this.DataContext as MainViewModel).AngleUpdator(BlueAndGreenSquare.Item1.Position, BlueAndGreenSquare.Item2.Position);

                RedLineService.DrawRedLine(ref RedLine);
            }
        }

        private void ImageOfTileGridMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Vector2<int> cordinats = (this.DataContext as MainViewModel).GetCordinats(Mouse.GetPosition(ImageOfTileGrid).X, Mouse.GetPosition(ImageOfTileGrid).Y);

            SquaresService.DrawSquare(Colors.Green, cordinats, BlueAndGreenSquare.Item2);

            if (BlueAndGreenSquare.Item1 != null & BlueAndGreenSquare.Item2 != null)
            {
                (this.DataContext as MainViewModel).AngleUpdator(BlueAndGreenSquare.Item1.Position, BlueAndGreenSquare.Item2.Position);

                RedLineService.DrawRedLine(ref RedLine);
            }
        }
    }
}