using System.Windows;
using System.Windows.Input;
using CollisionEditor.model;
using CollisionEditor.viewModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System;
using System.Windows.Media.Media3D;

namespace CollisionEditor
{   
    
    public partial class MainWindow : Window
    {
        private class RectagleAndPosition
        {
            public Rectangle Rectangle { get; set; } = new Rectangle(); 
            public Vector2<int> Position { get; set; } = new Vector2<int>();
        }

        RectagleAndPosition[] BlueAndGreenRectangle = new RectagleAndPosition[] { new RectagleAndPosition(), new RectagleAndPosition() };
        Rectangle RedLine = new Rectangle();
        public MainWindow()
        {
            InitializeComponent();
            MenuOpenAngleMap.Click += MenuOpenAngleMapClick;
            MenuOpenTileStrip.Click += MenuOpenTileStripClick;
            MenuSaveTiletmap.Click += MenuSaveTiletmapClick;
            DataContext = new MainViewModel(this);
        }

        private void MenuSaveTiletmapClick(object sender, RoutedEventArgs e)
        {

            
            if ((this.DataContext as MainViewModel).TileStripIsNull())
            {
                System.Windows.Forms.MessageBox.Show("Ошибка: Вы не выбрали Tilemap, чтобы её сохранить");
            }
            else
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();

                saveFileDialog.Filter = "Image Files(*.png)| *.png";

                //Спрашивать RowCount

                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    (this.DataContext as MainViewModel).SaveTileStrip(saveFileDialog.FileName);
                }
            }
        }

        private void MenuOpenAngleMapClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            string filePath = string.Empty;
            openFileDialog.Filter = "Binary Files(*.bin)| *.bin| All files(*.*) | *.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageOfTile.Source = null;
                filePath = openFileDialog.FileName;

                (this.DataContext as MainViewModel).OpenAngleMapFile(filePath);
            }
        }
        public static void ShowAnglemap(int angle256like, string hexAngle, double angle360like)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            
            mainWindow.TextBlock256Angle.Text = angle256like.ToString();
            mainWindow.TextBlockHexAngle.Text = hexAngle;
            mainWindow.TextBlock360Angle.Text = angle360like.ToString()+"'";
        }

        private void MenuOpenTileStripClick(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.png)| *.png| All files(*.*) | *.*";
            string filePath = string.Empty;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageOfTile.Source = null;
                filePath = openFileDialog.FileName;

                (this.DataContext as MainViewModel).OpenTileStripFile(filePath);
            }
        }
        public static void ShowTileStrip(System.Windows.Media.Imaging.BitmapSource TileStrip)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            mainWindow.ImageOfTile.Source = TileStrip;
        }

        private void ImageOfTileGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

            Vector2<int> cordinats = (this.DataContext as MainViewModel).GetCordinats(Mouse.GetPosition(mainWindow.ImageOfTileGrid).X, Mouse.GetPosition(mainWindow.ImageOfTileGrid).Y);
            if (cordinats.X >= 128)
                cordinats.X = 120;
            if (cordinats.Y >= 128)
                cordinats.Y = 120;

            Rectangle rect = new Rectangle();
            rect.Width = 8;
            rect.Height = 8;
            rect.Fill = new SolidColorBrush(Colors.Blue);

            canvasForRectangles.Children.Remove(BlueAndGreenRectangle[0].Rectangle);
            
            BlueAndGreenRectangle[0].Rectangle = rect;
            BlueAndGreenRectangle[0].Position = cordinats;

            Canvas.SetLeft(rect, cordinats.X);
            Canvas.SetTop(rect, cordinats.Y);

            canvasForRectangles.Children.Add(rect);
            if (BlueAndGreenRectangle[0] != null & BlueAndGreenRectangle[1] != null)
            {
                (this.DataContext as MainViewModel).AngleUpdator(BlueAndGreenRectangle[0].Position, BlueAndGreenRectangle[1].Position);

                DrawRedLine();
            }
        }
        private void DrawRedLine()
        {
            Rectangle line = new Rectangle();
            line.Width = 128*Math.Sqrt(2);
            line.Height = 1;
            line.Fill = new SolidColorBrush(Colors.Red);
            string stringAngle = TextBlock360Angle.Text.TrimEnd('\'');
            float floatAngle = float.Parse(stringAngle);
            if (floatAngle > 180)
            {
                floatAngle = floatAngle - 180;
            }
            RotateTransform rotateTransform1 = new RotateTransform(180-floatAngle);
            line.RenderTransformOrigin = new Point(0.5, 0.5);

            Canvas.SetTop(line, 64);
            line.RenderTransform = rotateTransform1;

            canvasForLine.Children.Remove(RedLine);
            RedLine = line;
            canvasForLine.Children.Add(line);
        }
        private void ImageOfTileGridMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

            Vector2<int> cordinats = (this.DataContext as MainViewModel).GetCordinats(Mouse.GetPosition(mainWindow.ImageOfTileGrid).X, Mouse.GetPosition(mainWindow.ImageOfTileGrid).Y);
            
            if (cordinats.X >= 128)
                cordinats.X = 120;
            if (cordinats.Y >= 128)
                cordinats.Y = 120;

            Rectangle rect = new Rectangle();
            rect.Width = 8;
            rect.Height = 8;
            rect.Fill = new SolidColorBrush(Colors.Green);

            canvasForRectangles.Children.Remove(BlueAndGreenRectangle[1].Rectangle);
            BlueAndGreenRectangle[1].Rectangle = rect;
            BlueAndGreenRectangle[1].Position = cordinats;

            Canvas.SetLeft(rect, cordinats.X);
            Canvas.SetTop(rect, cordinats.Y);

            canvasForRectangles.Children.Add(rect);

            if (BlueAndGreenRectangle[0]!=null & BlueAndGreenRectangle[1] != null)
            {

                (this.DataContext as MainViewModel).AngleUpdator(BlueAndGreenRectangle[0].Position, BlueAndGreenRectangle[1].Position);

                DrawRedLine();
            }
        }
        
    }
}